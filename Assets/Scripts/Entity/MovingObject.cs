using System.Collections;
using UnityEngine;

/// <summary>
///     An object, that moves.
/// </summary>
/// <remarks>Since this is an abstract class, it requires some actual implementation</remarks>
public abstract class MovingObject : MonoBehaviour
{
    private BoxCollider2D _boxCollider; //The BoxCollider2D component attached to this object.
    private float _inverseMoveTime; //Used to make movement more efficient.
    private Rigidbody2D _rb2D; //The Rigidbody2D component attached to this object.
    public LayerMask BlockingLayer; //Layer on which collision will be checked.
    protected bool IsMoving;
    public float MoveTime = 0.1f; //Time it will take object to move, in seconds.

    protected virtual void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        _boxCollider = GetComponent<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        _rb2D = GetComponent<Rigidbody2D>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        _inverseMoveTime = 1f / MoveTime;
    }


    /// <summary>
    ///     Determines if it is possible to move
    /// </summary>
    /// <param name="xDir">X direction</param>
    /// <param name="yDir">Y direction</param>
    /// <param name="hit">Raycast projection</param>
    /// <returns>true if able to move, false otherwise</returns>
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        // Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        var end = start + new Vector2(xDir, yDir);

        // Disable the boxCollider so that linecast doesn't hit this object's own collider.
        _boxCollider.enabled = false;

        // Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, BlockingLayer);

        // Re-enable boxCollider after linecast
        _boxCollider.enabled = true;

        // Check if anything was hit
        if (hit.transform == null)
        {
            // If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            StartCoroutine(SmoothMovement(end));
            //Return true to say that Move was successful
            return true;
        }

        // If something was hit, return false, Move was unsuccesful.
        return false;
    }

    /// <summary>
    /// Raycasts from this object's position in a line specified by xDir and yDir. Returns true if something in the
    /// blocking layer is hit. Returns false otherwise. Returns the hitting raycast out through the hit argument.
    /// </summary>
    /// <param name="xDir">The distance to cast in the x direction</param>
    /// <param name="yDir">The distance to cast in the y direction</param>
    /// <param name="hit">a RaycastHit2D object of whatever the raycast collides with. Null if nothing was hit.</param>
    /// <returns>True on a hit, false otherwise. Returns the object hit through the hit parameter.</returns>
    protected bool RaycastInDirection(int xDir, int yDir, out RaycastHit2D hit)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        _boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, BlockingLayer);

        //Re-enable boxCollider after linecast
        _boxCollider.enabled = true;

        //Check if anything was hit
        if (hit.transform == null)
        {
            return false;
        }
        //If something was hit, return false
        return true;
    }


    /// <summary>
    /// Co-routine for moving units from one space to next.
    /// </summary>
    /// <param name="end">The end point of movement</param>
    /// <returns></returns>
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        // Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        // Square magnitude is used instead of magnitude because it's computationally cheaper.
        var sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        GameManager.getInstance().IsMoving = true;

        // While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            var newPostion = Vector3.MoveTowards(_rb2D.position, end, _inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            _rb2D.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

        GameManager.getInstance().IsMoving = false;
    }

    /// <summary>
    ///     AttemptMove takes a generic parameter T to specify the type of component we expect our unit to interact with
    ///     if blocked (Player for Enemies, Wall for Player).
    /// </summary>
    /// <param name="xDir">X direction</param>
    /// <param name="yDir">Y direction</param>
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        // Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D hit;

        // Set canMove to true if Move was successful, false if failed.
        var canMove = Move(xDir, yDir, out hit);

        // Check if nothing was hit by linecast
        // Immediately returns if nothing was hit.
        if (hit.transform == null)
        {
            return;
        }

        // Get a component reference to the component of type T attached to the object that was hit
        var hitComponent = hit.transform.GetComponent<T>();

        // If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
        // Call the OnCantMove function and pass it hitComponent as a parameter.
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    /// <summary>
    ///     OnCantMove will be overriden by functions in the inheriting classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}