from PIL import Image

# 16x16 transparent Stardew-style inventory icon for the revolver.
# Keep the icon at native game sprite size; nearest-neighbour pixels are intentional.
rows = [
    "................",
    ".....DDDDDDDD...",
    "....DMMMMMMMD...",
    "...DMLLLLLMMMD..",
    "...DMMMMMMMMMDD.",
    "...DMMCCCMMMMMMD",
    "...DMMCCCMMLMMMD",
    "....DMMMMMMMMMD.",
    ".....DMMMMMMMD..",
    ".....DMMMMMD....",
    "....DBBBBDDD....",
    "...DBBBBBBD.....",
    "..DBBBRBBBD.....",
    "..DBBBBBBD......",
    "...DDDDDD.......",
    "................",
]

palette = {
    ".": (0, 0, 0, 0),
    "D": (20, 19, 24, 255),       # dark outline
    "M": (62, 60, 72, 255),       # dark steel
    "L": (143, 137, 155, 255),    # steel highlight
    "C": (94, 88, 108, 255),      # cylinder detail
    "B": (137, 76, 43, 255),      # wooden grip
    "R": (190, 112, 62, 255),     # grip highlight
}

img = Image.new("RGBA", (16, 16), (0, 0, 0, 0))
pixels = img.load()
for y, row in enumerate(rows):
    for x, symbol in enumerate(row):
        pixels[x, y] = palette[symbol]

img.save("assets/revolver.png")
