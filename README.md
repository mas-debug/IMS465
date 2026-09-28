# IMS465 Prototype 1.
The mechanic I prototyped from Week 1 is parkour from the Assassin's Creed franchise. I wanted to capture the essence of jumping from object to object with ease depending on where the player camera is looking. The mechanic tells me if an object I raycast to is interactable for parkour or not.

1. My early and late initialized code is in the PlayerController script. I used awake to initialize the character controller and the main camera object prior to any other code running. I used start for a few other variables that needed to be initialized.
2. Time.deltaTime was used in the movement method in the PlayerController script to control the speed of the movement.
3. I made the interface: "IInteractable" to find out if an object is interactable or not for parkour. Inside, I added a method that I used in the ParkourSurface script to run and tell the player what it is interacting with.
4. I used Unity's Input System to create movement and the character controller. (Note: I originally planned for a third person controller but I could only get the movement to work, then I tried a tutorial and it failed again. I finally swapped to first person in order to complete it.)

I had to shrink and cut from my original idea in week 1 due to knowledge constraints. I planned to make the character jump to boxes in unity, but I cut it down to a player character that works, and being able to raycast to objects to see if they are interactable or not. 
