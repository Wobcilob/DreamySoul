param(
    [Parameter(Mandatory = $true)]
    [string]$XnbPath,

    [Parameter(Mandatory = $true)]
    [string]$OutputDirectory,

    [Parameter(Mandatory = $true)]
    [string]$BaseName,

    [switch]$OnlyAtlas,

    [string]$AtlasFileName
)

$ErrorActionPreference = 'Stop'

function Read-7BitEncodedInt([System.IO.BinaryReader]$Reader) {
    $result = 0
    $shift = 0
    do {
        $value = $Reader.ReadByte()
        $result = $result -bor (($value -band 0x7f) -shl $shift)
        $shift += 7
    } while (($value -band 0x80) -ne 0)
    return $result
}

function Read-XnbString([System.IO.BinaryReader]$Reader) {
    $length = Read-7BitEncodedInt $Reader
    return [System.Text.Encoding]::UTF8.GetString($Reader.ReadBytes($length))
}

function Expand-Xnb([string]$Path) {
    $input = [System.IO.File]::OpenRead($Path)
    $reader = [System.IO.BinaryReader]::new($input)
    try {
        $magic = [System.Text.Encoding]::ASCII.GetString($reader.ReadBytes(3))
        if ($magic -ne 'XNB') {
            throw "Not an XNB file: $Path"
        }

        [void]$reader.ReadByte() # platform
        [void]$reader.ReadByte() # version
        $flags = $reader.ReadByte()
        $fileSize = $reader.ReadInt32()

        if (($flags -band 0x80) -eq 0) {
            return $reader.ReadBytes($fileSize - 10)
        }

        $decompressedSize = $reader.ReadInt32()
        $fnaPath = 'D:\steam\steamapps\common\tModLoader\Libraries\FNA\1.0.0\FNA.dll'
        $fna = [Reflection.Assembly]::LoadFrom($fnaPath)
        $decoderType = $fna.GetType('Microsoft.Xna.Framework.Content.LzxDecoder', $true)
        $decoder = $decoderType.GetConstructor(
            [Reflection.BindingFlags]'Instance,Public,NonPublic',
            $null,
            [Type[]]@([int]),
            $null
        ).Invoke(@(16))
        $decompress = $decoderType.GetMethod(
            'Decompress',
            [Reflection.BindingFlags]'Instance,Public,NonPublic'
        )

        $output = [System.IO.MemoryStream]::new($decompressedSize)
        $remaining = $fileSize - 14
        $decompressedRemaining = $decompressedSize
        while ($remaining -gt 0 -and $decompressedRemaining -gt 0) {
            $high = $reader.ReadByte()
            $low = $reader.ReadByte()
            $headerSize = 2
            $blockSize = (([int]$high -shl 8) -bor [int]$low)
            $frameSize = [Math]::Min(0x8000, $decompressedRemaining)

            if ($high -eq 0xff) {
                $frameSize = (([int]$low -shl 8) -bor [int]$reader.ReadByte())
                $blockSize = (([int]$reader.ReadByte() -shl 8) -bor [int]$reader.ReadByte())
                $headerSize = 5
            }

            $blockStart = $input.Position
            $decodeResult = $decompress.Invoke($decoder, @($input, $blockSize, $output, $frameSize))
            Write-Verbose "BlockSize=$blockSize FrameSize=$frameSize Result=$decodeResult InputPosition=$($input.Position) OutputPosition=$($output.Position)"
            $input.Position = $blockStart + $blockSize
            $remaining -= $blockSize + $headerSize
            $decompressedRemaining -= $frameSize
        }

        return $output.ToArray()
    }
    finally {
        $reader.Dispose()
        $input.Dispose()
    }
}

function Convert-XnbTextureToBitmap([byte[]]$Payload) {
    $stream = [System.IO.MemoryStream]::new($Payload, $false)
    $reader = [System.IO.BinaryReader]::new($stream)
    try {
        $readerCount = Read-7BitEncodedInt $reader
        for ($index = 0; $index -lt $readerCount; $index++) {
            [void](Read-XnbString $reader)
            [void]$reader.ReadInt32()
        }

        $sharedResourceCount = Read-7BitEncodedInt $reader
        if ($sharedResourceCount -ne 0) {
            throw 'This extractor expects a texture with no shared resources.'
        }

        [void](Read-7BitEncodedInt $reader) # root reader index
        $surfaceFormat = $reader.ReadInt32()
        $width = $reader.ReadInt32()
        $height = $reader.ReadInt32()
        $mipCount = $reader.ReadInt32()
        if ($surfaceFormat -ne 0) {
            throw "Unsupported SurfaceFormat: $surfaceFormat"
        }

        $dataLength = $reader.ReadInt32()
        $pixels = $reader.ReadBytes($dataLength)
        if ($pixels.Length -ne $width * $height * 4) {
            throw "Unexpected pixel data length: $($pixels.Length)"
        }

        $bitmap = [System.Drawing.Bitmap]::new(
            $width,
            $height,
            [System.Drawing.Imaging.PixelFormat]::Format32bppArgb
        )
        $offset = 0
        for ($y = 0; $y -lt $height; $y++) {
            for ($x = 0; $x -lt $width; $x++) {
                $red = $pixels[$offset]
                $green = $pixels[$offset + 1]
                $blue = $pixels[$offset + 2]
                $alpha = $pixels[$offset + 3]
                $bitmap.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($alpha, $red, $green, $blue))
                $offset += 4
            }
        }

        return $bitmap
    }
    finally {
        $reader.Dispose()
        $stream.Dispose()
    }
}

function Copy-TilePixels(
    [System.Drawing.Bitmap]$Source,
    [System.Drawing.Bitmap]$Destination,
    [int]$SourceX,
    [int]$SourceY,
    [int]$DestinationX,
    [int]$DestinationY
) {
    for ($y = 0; $y -lt 16; $y++) {
        for ($x = 0; $x -lt 16; $x++) {
            $Destination.SetPixel(
                $DestinationX + $x,
                $DestinationY + $y,
                $Source.GetPixel($SourceX + $x, $SourceY + $y)
            )
        }
    }
}

$payload = Expand-Xnb $XnbPath
Write-Verbose (($payload[0..([Math]::Min(95, $payload.Length - 1))] | ForEach-Object { $_.ToString('X2') }) -join ' ')
$atlas = Convert-XnbTextureToBitmap $payload
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null

try {
    if ([string]::IsNullOrWhiteSpace($AtlasFileName)) {
        $AtlasFileName = "$BaseName`_原始图集.png"
    }

    $atlasPath = Join-Path $OutputDirectory $AtlasFileName
    $atlas.Save($atlasPath, [System.Drawing.Imaging.ImageFormat]::Png)

    if ($OnlyAtlas) {
        "Atlas=$($atlas.Width)x$($atlas.Height)"
        "Output=$atlasPath"
        return
    }

    $positions = @(
        # 原版 TileFrame 对完整 3x3 连通矿块使用的九个帧位。
        @{ Name = '左上'; Column = 0; Row = 3; OutputColumn = 0; OutputRow = 0 },
        @{ Name = '上';   Column = 2; Row = 0; OutputColumn = 1; OutputRow = 0 },
        @{ Name = '右上'; Column = 5; Row = 3; OutputColumn = 2; OutputRow = 0 },
        @{ Name = '左';   Column = 0; Row = 2; OutputColumn = 0; OutputRow = 1 },
        @{ Name = '中心'; Column = 2; Row = 1; OutputColumn = 1; OutputRow = 1 },
        @{ Name = '右';   Column = 4; Row = 0; OutputColumn = 2; OutputRow = 1 },
        @{ Name = '左下'; Column = 2; Row = 4; OutputColumn = 0; OutputRow = 2 },
        @{ Name = '下';   Column = 1; Row = 2; OutputColumn = 1; OutputRow = 2 },
        @{ Name = '右下'; Column = 3; Row = 4; OutputColumn = 2; OutputRow = 2 }
    )

    $combined = [System.Drawing.Bitmap]::new(48, 48, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    try {
        foreach ($position in $positions) {
            $tile = [System.Drawing.Bitmap]::new(16, 16, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
            try {
                $sourceX = $position.Column * 18
                $sourceY = $position.Row * 18
                Copy-TilePixels $atlas $tile $sourceX $sourceY 0 0
                Copy-TilePixels $atlas $combined $sourceX $sourceY ($position.OutputColumn * 16) ($position.OutputRow * 16)
                $tile.Save(
                    (Join-Path $OutputDirectory "$BaseName`_$($position.Name).png"),
                    [System.Drawing.Imaging.ImageFormat]::Png
                )
            }
            finally {
                $tile.Dispose()
            }
        }

        $combined.Save(
            (Join-Path $OutputDirectory "$BaseName`_完整3x3.png"),
            [System.Drawing.Imaging.ImageFormat]::Png
        )
    }
    finally {
        $combined.Dispose()
    }

    "Atlas=$($atlas.Width)x$($atlas.Height)"
    "Output=$OutputDirectory"
}
finally {
    $atlas.Dispose()
}
