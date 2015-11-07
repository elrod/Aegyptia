using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    const float skinWidth = 0.15f;

    // public stuff
    public LayerMask collisionMask;         // We use this to establish which objects we want to collide with...

    public int horizontalRayCount = 4;      // This indicates how many ray we should cast along the horizontal sides of our box collider
    public int verticalRayCount = 4;        // This indicates how many ray we should cast along the vertical sides of our box collider

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

        // Passing a reference to our velocity instance...
        if (velocity.y != 0){
            VerticalCollisions(ref velocity);
        }
        if (velocity.x != 0){
            HorizontalCollisions(ref velocity);
        }

        transform.Translate(velocity);
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
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i + velocity.x);     // We are adding x velocity because we want to cast from the point we will be after moving in the x direction
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
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

        public void Reset(){
            above = below = false;
            left = right = false;
        }
    }
}
