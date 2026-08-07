extends Node

func _ready():
    # Connect Play button under MainMenu/UI/Button_Play -> _on_play_pressed
    var play_btn_path := "UI/Button_Play"
    if has_node(play_btn_path):
        var btn = get_node(play_btn_path)
        if typeof(btn) == TYPE_OBJECT and not btn.is_connected("pressed", self, "_on_play_pressed"):
            btn.connect("pressed", Callable(self, "_on_play_pressed"))

func _on_play_pressed():
    # Load the Level1 scene
    var scene_path := "res://scenes/Level1.tscn"
    if ResourceLoader.exists(scene_path):
        get_tree().change_scene_to_file(scene_path)
    else:
        push_error("Level scene not found: %s" % scene_path)

func on_level_complete():
    print("Level complete! Show win UI or load next level.")
