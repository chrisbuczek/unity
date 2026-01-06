1. Always separate logic from the visuals.
2. Grab references from the same game object in Awake() function. For accessing external references from different objects use Start() function.
3. You should generally use Update() for listening for key inputs and FixedUpdate() for physics.
