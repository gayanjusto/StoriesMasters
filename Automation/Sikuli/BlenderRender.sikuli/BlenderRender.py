#DEFINE REGIONS AND LOCATIONS
blenderObjSelectionRegion = Region(1271,183,318,208)
torsoLocation = Location(1326, 374)
legsLocation = Location(1328, 354)
headLocation = Location(1326, 333)
handsLocation = Location(1337, 314)
feetLocation = Location(1343, 295)
armsLocation = Location(1344, 272)

blenderArmatureLocation = Location(1335, 195)

blenderToolbarRegion = Region(1297,402,233,25)
blenderToolbarMaterialLocation = Location(1466, 413)
blenderToolbarRenderLocation = Location(1308, 413)

blenderRenderOptionsRegion = Region(1254,466,326,74)
blenderRenderAnimationButton = Location(1415, 504)

blenderMaterialSelectionRegion = Region(1257,458,317,203)
firstMaterialLocation = Location(1301, 487) #usually the material you want to assing to the object
secondMaterialLocation = Location(1305, 508) #can be either a material to hide the object (purple) or a second to assing to another part, such as left / right arm
thirdMaterialLocation = Location(1309, 530) #hidden material for parts that have more than 2 materials
assignButtonLocation = Location(1311, 591)


blender3DViewRegion = Region(45,120,498,555)
blenderLeftArm3DViewLocation1 = Location(316, 263) #we want to select the left arm in edit mode, so this is the first point for a mouse drag
blenderLeftArm3DViewLocation2 = Location(446, 394) #the drop part of the left arm selection

blenderLeftLeg3DViewLocation1 = Location(298, 260)
blenderLeftLeg3DViewLocation2 = Location(385, 516)

blenderLeftFoot3DViewLocation1 = Location(306, 460)
blenderLeftFoot3DViewLocation2 = Location(378, 562)


blenderFramesRegion = Region(545,823,95,30)
blenderFramesLocation = Location(589, 840)

#SET BLENDER ICON IMAGE
blenderIcon = "1497401162655.png"


#SET VARIABLES
bodyParts = ["Head", "Arms", "Hands", "Torso", "Legs", "Feet"]
bodyPartsLocation = [headLocation, armsLocation, handsLocation, torsoLocation, legsLocation, feetLocation]
basePlayerBodyPath = str("F:\\Projetos\\StoriesMasters\\2D Assets\\CombatWalking\\Player\\")
renderOutputPath = str("F:\\RenderOutpu")

previousBodyPart = 0


#ITERATION BETWEEN BODYPARTS
for bodyPartIndex in range(len(bodyParts)):
        currentBodyPart = bodyParts[bodyPartIndex]
        #RESET DESKTOP
        type("d", KeyModifier.WIN) #Windows + D

        #OPEN BLENDER
        click(blenderIcon)

        #Click to select bodypart in Blender
        print(bodyPartsLocation[bodyPartIndex])
        blenderObjSelectionRegion.click(bodyPartsLocation[bodyPartIndex])
        

        

        #Set material for bodyPart
        blenderToolbarRegion.click(blenderToolbarMaterialLocation)

        #assign material to the bodypart selected
        hover(blender3DViewRegion)
        keyDown(Key.TAB)
        keyUp(Key.TAB)
        type("z")
        wait(1)
        type("a")
        blenderMaterialSelectionRegion.click(firstMaterialLocation)
        blenderMaterialSelectionRegion.click(assignButtonLocation)

        #unselect all vertices
        hover(blender3DViewRegion)
        wait(1)
        type("a")
        
        #Remove wireframe view
        hover(blender3DViewRegion)
        type("z")      


        
        #if bodypart is arms, we must select the left arm only in edit mode in order to assign a different material to it
        if(currentBodyPart == "Arms" or currentBodyPart == "Legs" or currentBodyPart == "Feet"):
            hover(blender3DViewRegion)
            type("z")
            wait(1)
            
            type("b")
            if(currentBodyPart == "Arms"):
                dragDrop(blenderLeftArm3DViewLocation1, blenderLeftArm3DViewLocation2)
                blenderMaterialSelectionRegion.click(secondMaterialLocation)
                blenderMaterialSelectionRegion.click(assignButtonLocation)
            if(currentBodyPart == "Legs"):
                dragDrop(blenderLeftLeg3DViewLocation1, blenderLeftLeg3DViewLocation2)
                blenderMaterialSelectionRegion.click(secondMaterialLocation)
                blenderMaterialSelectionRegion.click(assignButtonLocation)
            if(currentBodyPart == "Feet"):
                dragDrop(blenderLeftFoot3DViewLocation1, blenderLeftFoot3DViewLocation2)
                blenderMaterialSelectionRegion.click(secondMaterialLocation)
                blenderMaterialSelectionRegion.click(assignButtonLocation)
                
            #Remove all selection
            hover(blender3DViewRegion)
            wait(1)
            type("a")
            
            #Remove wireframe view
            type("z")

            

        #exit edit mode
        keyDown(Key.TAB)
        keyUp(Key.TAB)

            
        angles = ["a1", "a2", "a3","a4", "a5", "a6", "a7", "a8" ]
        for angleIndex in range(len(angles)):  

            currentAngle = angles[angleIndex]
            #set output path
            renderBodyPartOutput = str(basePlayerBodyPath) + str(currentBodyPart) + "\\" + "\\" + str(currentAngle)
            
            print(renderBodyPartOutput)
    
            #Select Blender Armature
            blenderObjSelectionRegion.click(blenderArmatureLocation)
            hover(blender3DViewRegion)

            #rotate object in -45º
            type("r")
            type("z")
            type("-")
            type("45")
            keyDown(Key.ENTER)
            keyUp(Key.ENTER)

            #select render options from toolbar
            blenderToolbarRegion.click(blenderToolbarRenderLocation)

            
            #animate
            blenderRenderOptionsRegion.click(blenderRenderAnimationButton)

            wait(1)
            
            #open render output folder
            type("r", KeyModifier.WIN) #Windows + D
            wait(1)

            #paste folder path in windows run command
            type(str(renderOutputPath))
            keyDown(Key.ENTER)
            keyUp(Key.ENTER)
            wait(1)
            keyDown(Key.F5)
            keyUp(Key.F5)
            wait(1)

            #cut all rendered images
            keyDown(Key.CTRL)
            type("a")
            type("x")
            keyUp(Key.CTRL)

            #close render output window
            keyDown(Key.CTRL)
            type("w")
            keyUp(Key.CTRL)

            #open path to paste rendered images
            type("r", KeyModifier.WIN) #Windows + D
            wait(1)

             #paste folder path in windows run command
            type(str(renderBodyPartOutput))
            wait(1)
            keyDown(Key.ENTER)
            keyUp(Key.ENTER)
            wait(1)

            #paste images
            keyDown(Key.CTRL)
            type("v")
            keyUp(Key.CTRL)
            wait(1)

            #close pasted images folder    
            keyDown(Key.CTRL)
            type("w")
            keyUp(Key.CTRL)
            
            #RESET DESKTOP
            type("d", KeyModifier.WIN) #Windows + D
            wait(1)
            
            #OPEN BLENDER
            click(blenderIcon)

        #click bodypart again
        blenderObjSelectionRegion.click(bodyPartsLocation[bodyPartIndex])

        #Set material for bodyPart
        blenderToolbarRegion.click(blenderToolbarMaterialLocation)
        
        #Replace hidden material for bodypart
        hover(blender3DViewRegion)

        #enter edit mode
        keyDown(Key.TAB)
        keyUp(Key.TAB)
        wait(1)
        type("a")
        
        blenderMaterialSelectionRegion.click(secondMaterialLocation)

        if(currentBodyPart == "Arms" or currentBodyPart == "Legs" or currentBodyPart == "Feet"):
                    blenderMaterialSelectionRegion.click(thirdMaterialLocation)

        blenderMaterialSelectionRegion.click(assignButtonLocation)

        #Remove selection
        hover(blender3DViewRegion)
        wait(1)
        type("a")
        wait(1)

                
        
        


