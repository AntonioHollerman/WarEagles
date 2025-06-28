
using System;
using System.Collections;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AtomBehaviour
{
    public class Molecule : MonoBehaviour
    {
        public static float BaseSpeed = 5.0f;
        public float speedMultiplier;
        
        private Rigidbody2D _rb;
        private float _dirInDegrees;

        /// <summary>
        /// Checks if two GameObjects are currently colliding.
        /// This method requires both GameObjects to have a Collider component.
        /// For accurate results with dynamic objects, at least one GameObject should also have a Rigidbody.
        /// </summary>
        /// <param name="obj1">The first GameObject.</param>
        /// <param name="obj2">The second GameObject.</param>
        /// <returns>True if the colliders of the two GameObjects are currently touching; otherwise, false.</returns>
        public static bool IsColliding(GameObject obj1, GameObject obj2)
        {
            // 1. Get Colliders from both GameObjects.
            // TryGetComponent is generally safer as it doesn't throw an error if the component is missing.
            if (!obj1.TryGetComponent<Collider2D>(out Collider2D collider1))
            {
                Debug.LogWarning($"GameObject '{obj1.name}' is missing a Collider component. Cannot check collision.");
                return false;
            }

            if (!obj2.TryGetComponent<Collider2D>(out Collider2D collider2))
            {
                Debug.LogWarning($"GameObject '{obj2.name}' is missing a Collider component. Cannot check collision.");
                return false;
            }

            // 2. Use Collider.IsTouching() to check for collision.
            // This method accurately checks if the two colliders are physically overlapping.
            return collider1.IsTouching(collider2);
        }
        
        public static Vector3 DegreesToForward(float degrees)
        {
            // Convert degrees to radians, as Mathf.Cos and Mathf.Sin expect radians.
            float radians = degrees * Mathf.Deg2Rad;

            // Calculate the X and Y components of the unit vector for the given angle.
            // Cosine gives the X component, Sine gives the Y component.
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            // Return a new Vector3. The Z component is 0 because the original ForwardToDegrees
            // only considers the X and Y components for angle calculation.
            return new Vector3(x, y, 0f);
        }
        
        public static float ForwardToDegrees(Vector3 f)
        {
            float x = Mathf.Clamp(f.x, -1, 1);
            float y = Mathf.Clamp(f.y, -1, 1);
            float result;
            
            if (x != 0)
            {
                result = Mathf.Atan(y / x) * Mathf.Rad2Deg;
                if (x < 0)
                {
                    result += 180f;
                }
            } 
            else
            {
                result = Mathf.Asin(y) * Mathf.Rad2Deg;
            }
            
            if (result < 0)
            {
                result += 360;
            }
            return result;
        }

        public static float GetChangeInDir(float degrees)
        {
            degrees = NormalizeDeg(degrees);
            float degFromLastQuad = degrees;
            while (degFromLastQuad >= 90)
            {
                degFromLastQuad -= 90;
            }

            return 2 * degFromLastQuad;
        }
        
        public static Direction DegToDir(float degrees)
        {
            degrees = NormalizeDeg(degrees);
            if (120 >= degrees && degrees >= 60)
            {
                return Direction.Up;
            }

            if (240 > degrees && degrees > 120)
            {
                return Direction.Left;
            }

            if (300 >= degrees && degrees >= 240)
            {
                return Direction.Down;
            }
            
            return Direction.Right;
        }

        public static Quad DegToQuad(float degrees)
        {
            degrees = NormalizeDeg(degrees);
            if (degrees < 90)
            {
                return Quad.First;
            }

            if (degrees < 180)
            {
                return Quad.Second;
            }

            if (degrees < 270)
            {
                return Quad.Third;
            }
            
            return Quad.Fourth;
        }

        public static float NormalizeDeg(float degrees)
        {
            while (degrees < 0)
            {
                degrees += 360;
            }

            while (degrees >= 360)
            {
                degrees -= 360;
            }

            return degrees;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            ChangeDirection(other);
            StartCoroutine(CollisionCheck(other));
        }

        private IEnumerator CollisionCheck(Collision2D other)
        {
            GameObject obj1 = gameObject;
            GameObject obj2 = other.gameObject;

            yield return new WaitForSeconds(0.2f);
            while (IsColliding(obj1, obj2))
            {
                ChangeDirection(other);
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void ChangeDirection(Collision2D other)
        {
            int count = 0;
            float xSum = 0;
            float ySum = 0;

            if (other.contacts.Length == 0)
            {
                return;
            }
            foreach (ContactPoint2D contact in other.contacts)
            {
                // The 'point' property of a ContactPoint gives you the world position of the contact.
                Vector3 intersectionPoint = contact.point;

                count ++;
                xSum += intersectionPoint.x;
                ySum += intersectionPoint.y;
            }

            Vector3 avgIntersectionPoint = new Vector3(xSum / count, ySum / count, 0);
            Vector3 objToIntersectDir = Vector3.Normalize(avgIntersectionPoint - transform.position);

            float degrees = ForwardToDegrees(objToIntersectDir);
            Direction impactDir = DegToDir(degrees);
            
            float forwardDegrees = NormalizeDeg(ForwardToDegrees(transform.forward));
            Quad forwardQuad = DegToQuad(forwardDegrees);

            if (IsQuadAngle(forwardDegrees))
            {
                return;
            }

            float newDegrees = forwardDegrees;
            float changeInDir = GetChangeInDir(forwardDegrees);
            if (impactDir == Direction.Up || impactDir == Direction.Down)
            {
                if (forwardQuad == Quad.First || forwardQuad == Quad.Third)
                {
                    newDegrees -= changeInDir;
                }
                else
                {
                    newDegrees += changeInDir;
                }
            }
            
            if (impactDir == Direction.Right || impactDir == Direction.Left)
            {
                if (forwardQuad == Quad.First || forwardQuad == Quad.Third)
                {
                    newDegrees += changeInDir;
                }
                else
                {
                    newDegrees -= changeInDir;
                }
            }

            _dirInDegrees = NormalizeDeg(newDegrees);
        }

        private bool IsQuadAngle(float degrees)
        {
            degrees = NormalizeDeg(degrees);
            if (!(Mathf.Approximately(degrees, 0) || Mathf.Approximately(degrees, 90) || Mathf.Approximately(degrees, 180) || Mathf.Approximately(degrees, 270)))
            {
                return false;
            }

            float newDegrees = degrees;
            if (Mathf.Approximately(degrees, 0))
            {
                newDegrees = 180;
            }

            if (Mathf.Approximately(degrees, 90))
            {
                newDegrees = 270;
            }

            if (Mathf.Approximately(degrees, 180))
            {
                newDegrees = 0;
            }

            if (Mathf.Approximately(degrees, 270))
            {
                newDegrees = 90;
            }

            _dirInDegrees = newDegrees;
            return true;
        }
        
        private void Awake()
        {
            _dirInDegrees = Random.Range(0f, 360f); 
            _rb = GetComponent<Rigidbody2D>();
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.LookRotation(DegreesToForward(_dirInDegrees), Vector3.forward);
            _rb.velocity = BaseSpeed * speedMultiplier * transform.forward ;
        }
    }
}