using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Kazzymod.Projectiles.Minions
{
    class HydraBody : ModProjectile
    {
        private const float MaxDistanceWalk = 250;
        private const float MaxDistanceCatchUp = 600;
        private const float MaxDistanceTeleport = 2000;

        private const float WalkAcceleration = 0.2f;
        private const float MaxWalkSpeed = 10;
        private const float DragFactor = 0.95f;

        private const float GravityWeight = 0.5f;

        private const float FlyAcceleration = 0.5f;
        private const float MaxFlySpeed = 25;

        private State CurrentState
        {
            get { return (State)projectile.ai[0]; }
            set { projectile.ai[0] = (float)value; }
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 20;
            projectile.height = 20;
            projectile.penetrate = -1;
            projectile.timeLeft *= 5;
            projectile.minion = true;
            projectile.ignoreWater = true;
            projectile.friendly = true;           
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
        }

        public override void AI()
        {
            // projectile.ai[0] = 0; // Idle
            // projectile.ai[0] = 1; // Catching up
            // projectile.ai[0] = 2; // Attacking

            KazzyPlayer modPlayer = Main.player[projectile.owner].GetModPlayer<KazzyPlayer>();

            if (modPlayer.hydraMinion)
            {
                projectile.timeLeft = 2;
            }
            else
            {
                Kill(projectile.timeLeft);
                return;
            }

            //UpdateMovement();
         
        }

        private void UpdateMovement()
        {
            Player owner = Main.player[projectile.owner];

            float xDistance = owner.position.X + (owner.width / 2) - (projectile.position.X + (projectile.width / 2));
            float yDistance = owner.position.Y + (owner.height / 2) - (projectile.position.Y + (projectile.height / 2));
            var sqDistance = xDistance * xDistance + yDistance * yDistance;

            var direction = Math.Sign(xDistance);

            if (direction != 0)
            {
                projectile.direction = direction;
            }

            if (sqDistance > MaxDistanceTeleport * MaxDistanceTeleport)
            {
                projectile.position = owner.position;
                return;
            }

            if (CurrentState == State.Idle)
            {
                if (Math.Abs(xDistance) > MaxDistanceWalk)
                {
                    projectile.velocity.X += direction * WalkAcceleration;

                    if (Math.Abs(projectile.velocity.X) > MaxWalkSpeed)
                    {
                        projectile.velocity.X = MaxWalkSpeed;
                    }
                }

                Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
                projectile.velocity.Y += GravityWeight;

                projectile.velocity *= DragFactor;

                if (sqDistance > MaxDistanceCatchUp * MaxDistanceCatchUp)
                {
                    projectile.tileCollide = false;
                    CurrentState = State.CatchingUp;
                }
            }

            if (CurrentState == State.CatchingUp)
            {
                var distance = (float)Math.Sqrt(sqDistance);

                Vector2 normalizedDistance = new Vector2(xDistance / distance, yDistance / distance);
                projectile.velocity += normalizedDistance * FlyAcceleration;
                
                if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) > MaxFlySpeed)
                {
                    Vector2 normalizedVelocity = projectile.velocity / projectile.velocity.Length();
                    projectile.velocity.X *= MaxFlySpeed * normalizedVelocity.X;
                    projectile.velocity.Y *= MaxFlySpeed * normalizedVelocity.Y;
                }

                projectile.velocity *= DragFactor;

                if (sqDistance < MaxDistanceWalk * MaxDistanceWalk && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.tileCollide = true;
                    CurrentState = State.Idle;
                }
            }
        }

        public override bool MinionContactDamage()
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;

            return true;
        }

        private enum State
        {
            Idle,
            CatchingUp,
            Attacking
        }
    }
}
