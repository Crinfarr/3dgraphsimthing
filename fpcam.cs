using Godot;
using Godot.Collections;

public partial class fpcam : Camera3D
{
	const float mSensitivity = 0.7f;
	const float speed = 0.3f;
	Godot.Vector2 MouseDelta = new(0.0f, 0.0f);
	Dictionary<string, bool> keymap = new();

	float currTilt = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		foreach (string key in new[]{"w", "a", "s", "d", "space", "ctrl", "esc"})
			keymap[key] = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (Input.MouseMode == Input.MouseModeEnum.Captured) {
			MouseDelta *= mSensitivity;
			float pan = -MouseDelta.X;
			float tilt = -MouseDelta.Y;
			MouseDelta = new(0f,0f);
			tilt = Mathf.Clamp(tilt, -90 - currTilt, 90-currTilt);
			currTilt += tilt;
			RotateY(Mathf.DegToRad(pan));
			RotateObjectLocal(new Godot.Vector3(1, 0, 0), Mathf.DegToRad(tilt));

			if (keymap["esc"]) {
					GetTree().Quit(0);
			}

			Vector3 dir = new(
				(keymap["d"] ? 1.0f : 0.0f) - (keymap["a"] ? 1.0f : 0.0f),
				(keymap["space"] ? 1.0f : 0.0f) - (keymap["ctrl"] ? 1.0f : 0.0f),
				(keymap["s"] ? 1.0f : 0.0f) - (keymap["w"] ? 1.0f : 0.0f)
			);
			if (dir.Normalized() != Vector3.Zero)
				Translate(dir.Normalized() * speed);
		}
	}

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		
		if (@event is InputEventMouseMotion motion) {
			MouseDelta = motion.Relative;
		}

		if (@event is InputEventKey eventKey) {
			switch (eventKey.Keycode) {
				case Key.W:
					keymap["w"] = eventKey.Pressed;
					break;
				case Key.A:
					keymap["a"] = eventKey.Pressed;
					break;
				case Key.S:
					keymap["s"] = eventKey.Pressed;
					break;
				case Key.D:
					keymap["d"] = eventKey.Pressed;
					break;
				case Key.Ctrl:
					keymap["ctrl"] = eventKey.Pressed;
					break;
				case Key.Space:
					keymap["space"] = eventKey.Pressed;
					break;
				case Key.Escape:
					keymap["esc"] = eventKey.Pressed;
					break;
				default:
					break;
			}
		}
    }
}
