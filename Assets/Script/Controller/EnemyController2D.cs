using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyController2D : MonoBehaviour {

	const float skinWidth = 0.15f;
	
	// public stuff
	public LayerMask collisionMask;         // We use this to establish which objects we want to collide with...
	public LayerMask playerMask;
	
	public int horizontalRayCount = 4;      // This indicates how many ray we should cast along the horizontal sides of our box collider
	public int verticalRayCount = 4;        // This indicates how many ray we should cast along the vertical sides of our box collider

	public float seeAtDistance = 6f;
	
	float maxClimbAngle = 80f;
	float maxDescendAngle = 75f;
	
	public CollisionInfo collisions;
	
	
	// private stuff
	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	
	float horizontalRaySpacing;
	float verticalRaySpacing;
	
	
	
	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}
	
	// PUBLIC METHODS
	public void Move(Vector3 velocity){
		UpdateRaycastOrigins();
		collisions.Reset();
		
		collisions.velocityOld = velocity;
		collisions.oldPos = transform.position;
		
		if(velocity.y < 0){
			DescendSlope(ref velocity);
		}
		
		// Passing a reference to our velocity instance...
		if (velocity.x != 0){
			HorizontalCollisions(ref velocity);
			HorizontalPlayer(ref velocity);
		}
		if (velocity.y != 0){
			VerticalCollisions(ref velocity);
		}
		
		transform.Translate(velocity,Space.World);
		
	}
	
	// This takes a reference to the velocity vector, so that every change to the velocity vector, will affect the instance of velocity from caller
	void VerticalCollisions(ref Vector3 velocity){
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;
		
		for (int i = 0; i < verticalRayCount; i++){
			// If we are moving down we want to check collisions above us, if we are moving up we want to check collisions above us...
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);     // We are adding x velocity because we want to cast from the point we will be after moving in the x direction
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);
			
			if (hit){
				// Ok this should make avoid penetrating objects... this set the next velocity to the distance between
				// the ray origin and the hit point.
				velocity.y = (hit.distance - skinWidth) * directionY;
				// we should also update rayLength with the latest hit distance, to avoid cases in which a ray detects a collision
				// in a point that is closer to our character, but another ray detects another collision further up that is
				// more distance causing it to update velocity with an higher value, we don't want something like this:
				// PLAYER
				// ------
				// |    |
				// V    |
				// --   |
				//      V
				// -----------
				rayLength = hit.distance;
				
				// This should prevent our character to vibrate left to right when hitting an obstacle above him
				// while climbing a slope
				if (collisions.climbingSlope)
				{
					velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}
				
				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
		// Another little fix here... it appears that sometimes when we have two different slopes angles in the same climb,
		// Sometimes our character penetrate a little in the second slope causing it to stuck for a few frames
		// A quick fix is to cast another ray to check if the slopeAngle has changed, if so, we update our velocity.x
		// to avoid penetrating the new terrain and we update our slopeAngle!
		if (collisions.climbingSlope)
		{
			float directionX = Mathf.Sign(velocity.x);
			rayLength = Mathf.Abs(velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != collisions.slopeAngle)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}
	
	void HorizontalCollisions(ref Vector3 velocity){
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;
		
		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			/* we should add  "+ velocity.y" but it seems to generate problem, we should investigate this... */
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);     // We are adding x velocity because we want to cast from the point we will be after moving in the x direction
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);
			
			if (hit)
			{
				
				// We get the slope angle by checking the normal of the collision against a reference vector (for instance Vector2.up)
				// it's a little trigonometry :)
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if(i == 0 && slopeAngle <= maxClimbAngle)
				{
					// Ok this is quick fix to an issue that was showing up when
					// we were descending a slope, and meeting a climb immidiatly...
					// we tell our player that he's not descending anymore, but climbing again
					if (collisions.descendingSlope)
					{
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if(slopeAngle != collisions.slopeAngleOld)
					{
						distanceToSlopeStart = hit.distance - skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}
				if(!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;
					
					// This should prevent our character to vibrate up/down when hitting an obstacle on the side while climbing a slope
					if (collisions.climbingSlope)
					{
						velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}
					
					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
					collisions.slopeAngle = slopeAngle;
				}
				
			}
		}
	}

	void HorizontalPlayer(ref Vector3 velocity){
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = seeAtDistance;
		
		for (int i = 0; i < horizontalRayCount; i++){

			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			// we should add  "+ velocity.y" but it seems to generate problem, we should investigate this... 
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);     // We are adding x velocity because we want to cast from the point we will be after moving in the x direction
			RaycastHit2D hitH = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, playerMask);
			RaycastHit2D hitD = Physics2D.Raycast(rayOrigin, (Vector2.right * directionX) + Vector2.up, rayLength, playerMask);
			
			Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.green);
			Debug.DrawRay(rayOrigin, (Vector2.right * directionX) + Vector2.up, Color.green);
			
			if (hitH || hitD){
				collisions.enemyLeft = directionX == -1;
				collisions.enemyRight = directionX == 1;
			}
		} 
	}
	
	// This actually handles slope climbing...
	void ClimbSlope(ref Vector3 velocity, float slopeAngle){
		float moveDistance = Mathf.Abs(velocity.x);
		float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
		if (velocity.y <= climbVelocityY)
		{
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
		
	}
	
	void DescendSlope(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		// Let's cast another ray below us
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
		
		if (hit)
		{
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			// Checking wether we are no on a flat surface or in a too steep descend
			// in such cases we don't need to worry about descending slopes!
			if(slopeAngle != 0 && slopeAngle <= maxDescendAngle)
			{
				// We use the sign of the x component of the normal vector of the collision to establish the 
				// direction of the slope against our direction and see if we are actually descending
				if(Mathf.Sign(hit.normal.x) == directionX)
				{
					// Everything from here makes sense only if we are actually touching the slope, otherwise we are still falling
					if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
					{
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
						velocity.y -= descendVelocityY;
						
						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}
	
	void UpdateRaycastOrigins(){
		Bounds bounds = collider.bounds;
		// We are reducing a little bit the raycast origins position because we want rays to be cast
		// from a little bit inside our player sprite
		bounds.Expand(skinWidth * -2);
		
		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}
	
	void CalculateRaySpacing(){
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);
		
		// We want to be sure we cast at least two rays per side, so we clamp them to a range of [2, maxint] with the Clamp function
		// Raycasting is quite cheap, we can safely use as many as we need.
		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
		
		// Since we want rays to be equally distributed along their sides, we calculate spacing as:
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		
	}
	
	/* This struct define the origin positions of the rays we are going to cast to make collision detection *
     * these will likely be the boxCollider angles                                                          */
	struct RaycastOrigins{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
	
	/* Since we are selectevly doing collision detection, let's put in a structure informations about where we are colliding */
	public struct CollisionInfo{
		public bool above, below;
		public bool left, right;
		public bool enemyLeft, enemyRight;
		public bool climbingSlope;
		public bool descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld;
		public Vector3 oldPos;
		
		public void Reset(){
			above = below = false;
			left = right = false;
			enemyLeft = enemyRight = false;
			climbingSlope = false;
			descendingSlope = false;
			slopeAngleOld = slopeAngle;
			slopeAngle = 0f;
		}
	}
}
