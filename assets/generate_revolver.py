from PIL import Image

# 16x16 transparent pixel-art weapon sprite.
img = Image.new("RGBA", (16, 16), (0, 0, 0, 0))
p = img.load()

steel = (35, 38, 43, 255)
light = (86, 91, 98, 255)
dark = (17, 19, 23, 255)
brown = (104, 61, 35, 255)
brown_light = (151, 94, 52, 255)
black = (8, 9, 11, 255)

pixels = {
    (3,5): dark, (4,5): steel, (5,5): light, (6,5): steel,
    (7,5): steel, (8,5): light, (9,5): steel, (10,5): steel,
    (11,5): dark, (12,5): black, (13,5): black,
    (3,6): steel, (4,6): light, (5,6): steel, (6,6): dark,
    (7,6): steel, (8,6): steel, (9,6): light, (10,6): steel,
    (11,6): steel, (12,6): dark, (13,6): black,
    (4,7): dark, (5,7): steel, (6,7): light, (7,7): steel,
    (8,7): dark, (9,7): steel, (10,7): dark, (11,7): dark,
    (5,8): dark, (6,8): steel, (7,8): light, (8,8): dark,
    (9,8): brown, (10,8): brown_light, (11,8): brown,
    (4,9): black, (5,9): brown, (6,9): brown_light, (7,9): brown,
    (8,9): brown, (9,9): brown_light, (10,9): brown,
    (5,10): black, (6,10): brown, (7,10): brown_light, (8,10): brown,
    (9,10): brown, (10,10): black,
    (6,11): black, (7,11): brown, (8,11): brown_light, (9,11): black,
}
for (x, y), color in pixels.items():
    p[x, y] = color

img.save("assets/revolver.png")
