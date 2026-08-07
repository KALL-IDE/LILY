extends CharacterBody2D

@export var speed := 220

func _physics_process(delta):
    var dir = Vector2.ZERO
    dir.x = Input.get_action_strength("ui_right") - Input.get_action_strength("ui_left")
    dir.y = Input.get_action_strength("ui_down") - Input.get_action_strength("ui_up")
    if dir != Vector2.ZERO:
        velocity = dir.normalized() * speed
    else:
        velocity = Vector2.ZERO
    velocity = move_and_slide(velocity)

# Touch input helper (simple)
func _unhandled_input(event):
    # You can map touch UI buttons to ui_left/ui_right etc. in the Input Map
    pass
