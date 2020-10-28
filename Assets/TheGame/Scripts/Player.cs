using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace TheGame.Scripts
{
    /// <summary>
    /// Controlling the Player game object
    /// </summary>
    public class  Player : MonoBehaviour
    {
        
        /// <summary>
        /// Gibt an ob die Spielfigur auf dem Boden steht oder nicht.
        /// Wenn false, dann entweder springt oder fällt sie.
        /// </summary>
        public bool onGround = false;
        
        /// <summary>
        /// Running speed of Player game object
        /// </summary>
        public float speed = 0.05f;
        
        /// <summary>
        /// Die Kraft mit der nach oben gesprungen wird.
        /// </summary>
        public float jumpPush = 4f;
        
        /// <summary>
        /// Verstärkung der Gravitation, damit die Figur schneller fällt
        /// </summary>
        public float extraGravity = -20f;

        /// <summary>
        /// Das grafische Modell, u.a. für die Drehung in Laufrichtung
        /// </summary>
        public GameObject model;

        /// <summary>
        /// Der Winkel um den sich die Figur um die eigene Achse (=y) drehen soll
        /// </summary>
        private float towardsY = 0f;
        
        /// <summary>
        /// Zeiger auf die Physik-Komponente
        /// </summary>
        private Rigidbody rigid;

        /// <summary>
        /// Zeiger auf die Animations-Komponente
        /// </summary>
        private Animator anim;

        private void Start()
        {
            // hier werden Komponente vom Game Objekt initialisiert
            rigid = GetComponent<Rigidbody>();
            anim = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            
            float horizontal = Input.GetAxis("Horizontal"); // Eingabesignal für das Laufen
            anim.SetFloat("forward", Mathf.Abs(horizontal));
            
            // Vorwärts bewegen
            transform.position += horizontal * speed * transform.forward;
            
            // Drehen
            if (horizontal > 0f)
            {
                // Zielwert
                towardsY = 0f;
            }
            else if (horizontal < 0f)
            {
                // Zielwert
                towardsY = -180f;
            }

            model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.Euler(0f, towardsY, 0f),
                Time.deltaTime*10f);

            RaycastHit hitInfo;
            onGround = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.12f);
            
            anim.SetBool("grounded", onGround);
            // Springen:
            if(Input.GetAxis("Jump") > 0f && onGround)
            {
                Vector3 power = rigid.velocity; // die Richtung in die es sich das Objekt durch die Physiksimulation bewegt, wir kopieren die momentane Bewegung im Raum
                power.y = jumpPush; // die Bewegung nach oben soll zusätzlich die Stärke 'jumpPush bekommen'
                rigid.velocity = power;
            }
            
           // rigid.AddForce(new Vector3(0f, extraGravity, 0f));
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Vector3 rayStartPosition = transform.position + (Vector3.up * 0.1f);
            Gizmos.DrawLine(rayStartPosition, rayStartPosition + (Vector3.down * 0.12f));
        }
        
    }
}
