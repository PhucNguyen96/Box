using UnityEngine;

public class Box : MonoBehaviour
{
    public float moveSpeed = 2.0f;

    public Rigidbody2D rb;

    public AudioSource boxCollisionSound;
    public AudioSource boxExploisionSound;
    public AudioSource dropSound;

    float x_Range = 2.4f;
    bool isDropped;
    [HideInInspector]
    public bool ignoreCollision;
    bool ignoreTrigger;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        isDropped = false;
        ignoreCollision = false;
        ignoreTrigger = false;

        System.Random rand = new System.Random();
        moveSpeed = moveSpeed * (rand.Next(0, 2) * 2 - 1);

        GameController.instance.currentBox = this;
        transform.parent = GameController.instance.boxSpawner.transform;
    }

    void Update()
    {
        MoveBox();
    }

    void MoveBox()
    {
        if (!isDropped)
        {
            if (GameController.instance.isGameOver)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 temp = transform.position;
            temp.x += moveSpeed * Time.deltaTime;
            transform.position = temp;

            if (temp.x > x_Range)
            {
                moveSpeed = -moveSpeed;
            }

            if ( temp.x < -x_Range)
            {
                moveSpeed = -moveSpeed;
            }
        }
    }

    public void DropBox()
    {
        if (!isDropped)
        {
            isDropped = true;
            dropSound.Play();
            rb.gravityScale = 2.0f;
        }
    }

    private void Landed()
    {
        if (GameController.instance.isGameOver)
        {
            return;
        }

        GameController.instance.boxes.Add(this);

        if (GameController.instance.boxes.Count >= 2)
        {
            int length = GameController.instance.boxes.Count;
            GameController.instance.boxes[length - 1].ignoreTrigger = false;
            GameController.instance.boxes[length - 2].ignoreTrigger = true;
        }

        GameController.instance.SpawnNewBox();
        GameController.instance.MoveCamera();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ignoreCollision)
        { 
            return;
        }

        if (collision.gameObject.tag == "Holder" || collision.gameObject.tag == "Box")
        {
            boxCollisionSound.Play();
            ignoreCollision = true;
            Landed();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreTrigger || GameController.instance.isGameOver)
        {
            return;
        }

        if (collision.gameObject.tag == "GameOver")
        {
            GameController.instance.RestartGame();

            GameController.instance.isGameOver = true;

            boxExploisionSound.Play();


        }
    }
}
