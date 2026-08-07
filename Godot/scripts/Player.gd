extends CharacterBody2D

@export var speed: float = 220.0

func _physics_process(delta: float) -> void:
    var dir := Vector2.ZERO
    dir.x = Input.get_action_strength("ui_right") - Input.get_action_strength("ui_left")
    dir.y = Input.get_action_strength("ui_down") - Input.get_action_strength("ui_up")

    if dir != Vector2.ZERO:
        velocity = dir.normalized() * speed
    else:
        velocity = Vector2.ZERO

    # In Godot 4, call move_and_slide() with no arguments when using CharacterBody2D
    move_and_slide()

func _unhandled_input(event: InputEvent) -> void:
    # Placeholder for touch handling if needed
    pass
