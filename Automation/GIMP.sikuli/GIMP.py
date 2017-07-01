#ICONS
gimpIcon = "1497549151350.png"


#REGIONS AND LOCATIONS
openFileRegion = Region(358,234,152,316)
partitionLocation = Location(418, 470)

fileLocationRegion = Region(354,234,342,35)
fileLocationPathLocation = Location(474, 254)

gimpImageRegion = Region(714,429,68,73)
bodyPartsHiddenMaterialLocation = Location(743, 442)
headHiddenMaterialLocation = Location(744, 457)

basePathToImages = "F:\\Projetos\\StoriesMasters\\2D Assets\\\SwingAttack_1\\Player"

bodyParts = ["Head", "Arms", "Hands", "Torso", "Legs", "Feet"]
angles = ["a1", "a2","a3","a4","a5","a6","a7","a8"]
frames = [0,1,2,3,4,5,6]

amountImg = 0
for bodyPartIndex in range(len(bodyParts)):
    currentBodyPart = str(bodyParts[bodyPartIndex])
    for angleIndex in range(len(angles)):
        for frame in frames:
            currentFrame = "0" + str(frame)
            pathToImage = basePathToImages + "\\" + str(currentBodyPart) + "\\" + str(angles[angleIndex]) + "\\" + str(currentFrame) + ".png"

            #open image hotkeys
            keyDown(Key.CTRL)
            type("o")
            keyUp(Key.CTRL)

            #click on ''F'' partition
            openFileRegion.click(partitionLocation)

            #set path to image
            fileLocationRegion.click(fileLocationPathLocation)

            #select entire path and delete it
            keyDown(Key.CTRL)
            type("a")
            keyUp(Key.CTRL)
            type(Key.DELETE)

            #paste path
            type(str(pathToImage))
            
            #open image
            type(Key.ENTER)

            wait(1)
            #select hidden path material by color
            keyDown(Key.SHIFT)
            type("o")
            keyUp(Key.SHIFT)

            if(currentBodyPart == "Head"):
                gimpImageRegion.click(headHiddenMaterialLocation)
            else:
                gimpImageRegion.click(bodyPartsHiddenMaterialLocation)

            wait(1)
            #delete hidden material
            type(Key.DELETE)

            #save image
            keyDown(Key.CTRL)
            keyDown(Key.SHIFT)
            type("e")
            keyUp(Key.CTRL)
            keyUp(Key.SHIFT)

            type(Key.ENTER)
            wait(1)
            
            #confirm
            type(Key.ENTER)
            wait(1)
            
            #confirm export
            type(Key.ENTER)

            #close image
            keyDown(Key.CTRL)
            type("w")
            keyUp(Key.CTRL)
            wait(1)
            
            #confirm discard changes
            keyDown(Key.ALT)
            type("d")
            keyUp(Key.ALT)
            wait(1)

            #loop
            

            
            