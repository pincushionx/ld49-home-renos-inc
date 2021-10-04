using System;

namespace Pincushion.LD49
{
    public static class IUtil
    {
        /// <summary>
        /// Rotate an IVector3 according to a given direction. This is a "Look At" rotation.
        /// </summary>
        /// <param name="subject">The return value</param>
        /// <param name="upValues">The IVector3 to rotate, in its "Up" position</param>
        /// <param name="direction">The direction to rotate. Expecting x,y,z values of -1,0 or 1</param>
        public static void LookAt(IVector3 subject, IVector3 upValues, IVector3 direction)
        {
            subject = RotateToward(subject, upValues, direction);
        }
        public static IVector3 RotateToward(IVector3 subject, IVector3 upValues, IVector3 direction)
        {

            if (direction.x != 0 && direction.y != 0 && direction.z != 0)
            {
                // 
                subject.x = (int)((upValues.x + upValues.y + upValues.z) / 3f);
                subject.y = (int)((upValues.x + upValues.y + upValues.z) / 3f);
                subject.z = (int)((upValues.x + upValues.y + upValues.z) / 3f);

                subject.x *= Math.Sign(direction.x);
                subject.y *= Math.Sign(direction.y);
                subject.z *= Math.Sign(direction.z);
            }
            else if (direction.x != 0 && direction.z != 0)
            {
                // 
                subject.x = (int)(upValues.x / 2f + upValues.y / 2f);
                subject.y = (int)(upValues.x / 2f + upValues.z / 2f);
                subject.z = (int)(upValues.z / 2f + upValues.y / 2f);

                subject.x *= Math.Sign(direction.x);
                subject.z *= Math.Sign(direction.z);
            }
            else if (direction.x != 0 && direction.y != 0)
            {
                // 
                subject.x = (int)(upValues.x / 2f + upValues.z / 2f);
                subject.y = (int)(upValues.y / 2f + upValues.z / 2f);
                subject.z = (int)(upValues.x / 2f + upValues.y / 2f);

                subject.x *= Math.Sign(direction.x);
                subject.y *= Math.Sign(direction.y);
            }
            else if (direction.y != 0 && direction.z != 0)
            {
                // 
                subject.x = (int)(upValues.y / 2f + upValues.z / 2f);
                subject.y = (int)(upValues.y / 2f + upValues.x / 2f);
                subject.z = (int)(upValues.z / 2f + upValues.x / 2f);

                subject.x *= Math.Sign(direction.x);
                subject.y *= Math.Sign(direction.y);
            }
            else if (direction.y != 0)
            {
                // x,z
                subject.x = upValues.x;
                subject.y = upValues.y;
                subject.z = upValues.z;

                subject.y *= Math.Sign(direction.y);
            }
            else if (direction.x != 0)
            {
                // x,y
                subject.x = upValues.y;
                subject.y = upValues.x;
                subject.z = upValues.z;

                subject.x *= Math.Sign(direction.x);
            }
            else if (direction.z != 0)
            {
                // x,y
                subject.x = upValues.x;
                subject.y = upValues.z;
                subject.z = upValues.y;

                subject.z *= Math.Sign(direction.z);
            }
            return subject;
        }

        /// <summary>
        /// Rotate an IBound3 according to a given direction
        /// </summary>
        /// <param name="subject">The return value</param>
        /// <param name="upValues">The IBound3 to rotate, in its "Up" position</param>
        /// <param name="direction">The direction to rotate. Expecting x,y,z values of -1,0 or 1</param>
        public static IBound3 RotateToward(IBound3 subject, IBound3 upValues, IVector3 direction)
        {
            subject.position = RotateToward(subject.position, upValues.position, direction);
            subject.size = RotateToward(subject.size, upValues.size, direction);

            if (subject.size.x < 0)
            {
                subject.position.x += subject.size.x;
                subject.size.x *= -1;
            }
            if (subject.size.y < 0)
            {
                subject.position.y += subject.size.y;
                subject.size.y *= -1;
            }
            if (subject.size.z < 0)
            {
                subject.position.z += subject.size.z;
                subject.size.z *= -1;
            }
            return subject;
        }

        /// <summary>
        /// Enlarge the given ibound by the given size. The size will be applied on all sides
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="increaseSize"></param>
        public static IBound3 EnlargeIBound(IBound3 subject, int increaseSize)
        {
            subject.position.x -= increaseSize;
            subject.position.y -= increaseSize;
            subject.position.z -= increaseSize;

            subject.size.x += increaseSize * 2;
            subject.size.y += increaseSize * 2;
            subject.size.z += increaseSize * 2;

            return subject;
        }
        public static IBound2 EnlargeIBound(IBound2 subject, int increaseSize)
        {
            subject.position.x -= increaseSize;
            subject.position.y -= increaseSize;

            subject.size.x += increaseSize * 2;
            subject.size.y += increaseSize * 2;

            return subject;
        }

        public static void Clamp(ref IBound3 subject, IVector3 min, IVector3 max)
        {
            //TODO size
            Clamp(ref subject.position, min, max);
        }
        public static void Clamp(ref IVector3 subject, IVector3 min, IVector3 max)
        {
            //TODO max
            if (subject.x < min.x)
            {
                subject.x = min.x;
            }
            if (subject.y < min.y)
            {
                subject.y = min.y;
            }
            if (subject.z < min.z)
            {
                subject.z = min.z;
            }
        }
        public static IBound3 Rotate( IBound3 subject, IVector3 rotation)
        {
            subject.position = Rotate(subject.position, rotation);
            subject.size = Rotate(subject.size, rotation);

            if (subject.size.x < 0)
            {
                subject.position.x += subject.size.x;
                subject.size.x *= -1;
            }
            if (subject.size.y < 0)
            {
                subject.position.y += subject.size.y;
                subject.size.y *= -1;
            }
            if (subject.size.z < 0)
            {
                subject.position.z += subject.size.z;
                subject.size.z *= -1;
            }
            return subject;
        }

        public static void Abs(ref IVector3 subject)
        {
            if (subject.x < 0)
            {
                subject.x *= -1;
            }
            if (subject.y < 0)
            {
                subject.y *= -1;
            }
            if (subject.z < 0)
            {
                subject.z *= -1;
            }
        }

        public static IVector3 Rotate(IVector3 subject, IVector3 rotation)
        {
            int tmp;

            if (rotation.x != 0 || rotation.y != 0 || rotation.z != 0)
            {
                if (rotation.x == 1)
                {
                    tmp = subject.y;
                    subject.y = subject.z;
                    subject.z = -tmp;
                }
                else if (rotation.x == -1)
                {
                    tmp = subject.y;
                    subject.y = -subject.z;
                    subject.z = tmp;
                }
                else if (rotation.x == 2)
                {
                    subject.y = -subject.y;
                    subject.z = -subject.z;
                }

                if (rotation.y == 1)
                {
                    tmp = subject.x;
                    subject.x = subject.z;
                    subject.z = -tmp;
                }
                else if (rotation.y == -1)
                {
                    tmp = subject.x;
                    subject.x = -subject.z;
                    subject.z = tmp;
                }
                else if (rotation.y == 2)
                {
                    subject.x = -subject.x;
                    subject.z = -subject.z;
                }

                if (rotation.z == 1)
                {
                    tmp = subject.x;
                    subject.x = subject.y;
                    subject.y = -tmp;
                }
                else if (rotation.z == -1)
                {
                    tmp = subject.x;
                    subject.x = -subject.y;
                    subject.y = tmp;
                }
                else if (rotation.z == 2)
                {
                    subject.x = -subject.x;
                    subject.y = -subject.y;
                }
            }
            return subject;
        }

        public static IVector3 SanitizeRotation(IVector3 rotation)
        {
            rotation.x = rotation.x % 4;

            if (rotation.x == 3)
            {
                rotation.x = -1;
            }
            if (rotation.x == -3)
            {
                rotation.x = 1;
            }
            if (rotation.x == -2)
            {
                rotation.x = 2;
            }

            rotation.y = rotation.y % 4;

            if (rotation.y == 3)
            {
                rotation.y = -1;
            }
            if (rotation.y == -3)
            {
                rotation.y = 1;
            }
            if (rotation.y == -2)
            {
                rotation.y = 2;
            }

            rotation.z = rotation.z % 4;

            if (rotation.z == 3)
            {
                rotation.z = -1;
            }
            if (rotation.z == -3)
            {
                rotation.z = 1;
            }
            if (rotation.z == -2)
            {
                rotation.z = 2;
            }

            return rotation;
        }

        public static IVector3 InvertRotation(IVector3 rotation)
        {
            return new IVector3(
                (rotation.x == 1 || rotation.x == -1) ? rotation.x * -1 : rotation.x,
                (rotation.y == 1 || rotation.y == -1) ? rotation.y * -1 : rotation.y,
                (rotation.z == 1 || rotation.z == -1) ? rotation.z * -1 : rotation.z
            );
        }
    }
}