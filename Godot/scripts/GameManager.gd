extends Node

func _ready():
    # Connect Play button if present
    if has_node("../Button_Play"):
        var btn = get_node("../Button_Play")
        btn.connect("pressed", Callable(self, "_on_play_pressed"))

func _on_play_pressed():
    if Engine.has_singleton("SceneTree"):
        get_tree().change_scene_to_file("res://scenes/Level1.tscn")

func on_level_complete():
    print("Level complete! Show win UI or load next level.")
