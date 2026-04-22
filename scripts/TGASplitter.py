"""
TGASpliter - Splits a TGA sprite sheet into individual JPG sprites.

Usage:
    python TGASpliter.py <tga_file> <cols> <rows> <output_prefix>

Example:
    python TGASpliter.py sprites.tga 4 4 output/block
    → output/block_1.jpg ~ output/block_16.jpg
"""

import sys
from pathlib import Path

try:
    from PIL import Image
except ImportError:
    print("Pillow is required. Install with: pip install Pillow")
    sys.exit(1)


def main():
    if len(sys.argv) != 5:
        print(f"Usage: python {sys.argv[0]} <tga_file> <cols> <rows> <output_prefix>")
        print(f"Example: python {sys.argv[0]} sprites.tga 4 4 output/block")
        sys.exit(1)

    tga_path = Path(sys.argv[1])
    cols = int(sys.argv[2])
    rows = int(sys.argv[3])
    output_prefix = sys.argv[4]

    if not tga_path.exists():
        print(f"File not found: {tga_path}")
        sys.exit(1)

    img = Image.open(tga_path).convert("RGBA")
    img_w, img_h = img.size
    sprite_w = img_w // cols
    sprite_h = img_h // rows

    # Create output directory
    out_dir = Path(output_prefix).parent
    out_dir.mkdir(parents=True, exist_ok=True)

    index = 1
    for row in range(rows):
        for col in range(cols):
            left = col * sprite_w
            upper = row * sprite_h
            right = left + sprite_w
            lower = upper + sprite_h

            sprite = img.crop((left, upper, right, lower))
            output_path = Path(f"{output_prefix}_{index}.jpg")
            sprite.convert("RGB").save(output_path, "JPEG")
            index += 1

    print(f"Split {tga_path.name} ({img_w}x{img_h}) into {cols*rows} sprites ({sprite_w}x{sprite_h} each)")


if __name__ == "__main__":
    main()
