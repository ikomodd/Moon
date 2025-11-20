shader_type canvas_item;

uniform sampler2D TextureAlbedo;
uniform float CutOff : hint_range(0.0, 1.0, 0.1);

void vertex() {

	vec4 TextureColor = texture(TextureAlbedo, UV);

	if (TextureColor.a <= CutOff) COLOR = vec4(0.0);
	else COLOR = vec4(0.0, 0.0, 0.0, 0.5);

}

void fragment() {

}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
