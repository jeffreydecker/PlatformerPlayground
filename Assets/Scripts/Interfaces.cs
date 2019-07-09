using UnityEngine;
using System;

public interface IPlatformDimensions {
	/// <summary>
	/// Provides the width of a platform.
	/// </summary>
	/// <returns>
	/// The width of a platform. 
	/// </returns>
	float Width ();

	/// <summary>
	/// Provides a value to help place a platform horizontally. If multiple grounds
	/// are contained within a platform the "main level" should determine the offset.
	/// </summary>
	/// <example>
	///	If the visible center of the "main level" platform is shifted 1 unit to the
	///	left of its objects anchor, then this method should return <c>-1</c>.
	/// </example>
	/// <returns>
	/// The horrizontal offset from the center of a platform from its anchor;
	/// </returns>
	float HorrizontalOffset ();

	/// <summary>
	/// Provides a value to help place a platform vertically. If multiple grounds
	/// are contained within a platform the "main level" should determine the offset.
	/// </summary>
	/// <example>
	///	If the "main level" ground collider is shifted 2.5 units above the platform
	///	anchor, then this method should return <c>2.5</c>.
	/// </example>
	/// <returns>
	/// The vertical offset from the ground of a platform from its anchor
	/// </returns>
	float VerticalOffset ();

	/// <summary>
	/// Provides the left most x and "main" ground level y coordinates.
	/// </summary>
	/// <returns>
	/// The left most x and "main" ground level y coordinates.
	/// </returns>
	Vector2 LeftBound ();

	/// <summary>
	/// Provides the right most x and "main" ground level y coordinates.
	/// </summary>
	/// <returns>
	/// The right most x and "main" ground level y coordinates.
	/// </returns>
	Vector2 RightBound ();
}

public interface IDestroyable {
	void Destroy ();
}