"""
SpriteConverter - Converts sprite JPG images to C# byte array format (BGRA).

Usage:
    python SpriteConverter.py <image_path> [image_path2 ...]

Output:
    For each input image, creates a .txt file in the same directory
    with the pixel data in BGRA format matching BlockPixelData.cs style.
"""

import sys
from pathlib import Path

try:
    from PIL import Image
except ImportError:
    print("Pillow is required. Install with: pip install Pillow")
    sys.exit(1)


def convert_image(image_path: str) -> None:
    path = Path(image_path)
    if not path.exists():
        print(f"File not found: {path}")
        return

    img = Image.open(path).convert("RGBA")
    width, height = img.size
    pixels = img.load()

    lines = []
    for y in range(height):
        row_values = []
        for x in range(width):
            r, g, b, a = pixels[x, y]
            # BGRA order (matching WinUI 3 / Direct2D format)
            row_values.extend([b, g, r, a])
        lines.append("\t" + ", ".join(str(v) for v in row_values) + ",")

    output_path = path.with_suffix(".txt")
    output = "{\n" + "\n".join(lines) + "\n}"

    output_path.write_text(output)
    print(f"Converted: {path.name} ({width}x{height}) -> {output_path.name}")


def main():
    if len(sys.argv) < 2:
        print(f"Usage: python {sys.argv[0]} <image_path> [image_path2 ...]")
        sys.exit(1)

    for image_path in sys.argv[1:]:
        convert_image(image_path)


if __name__ == "__main__":
    main()
