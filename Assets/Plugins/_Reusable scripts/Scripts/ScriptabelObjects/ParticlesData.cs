using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "ParticlesData", menuName = "ScriptableObjects/Utils/ParticlesData")]

    public class ParticlesData : ScriptableObject
    {
        [SerializeField] private List<ParticleData> particlesData;

        public ParticleData this[Particles particleName]
        {
            get
            {
                foreach (ParticleData particleData in particlesData)
                {
                    if (particleData.ParticleName == particleName)
                    {
                        return particleData;
                    }
                }

                return null;
            }
        }
    }

    [Serializable]
    public class ParticleData
    {
        [SerializeField] private Particles particleName;
        [SerializeField] private GameObject particleObj;
        [SerializeField] private float scale = 1f;
        [SerializeField] private Vector3 offset;

        public Particles ParticleName { get { return particleName; } }
        public GameObject Prefab { get { return particleObj; } }
        public Vector3 Scale { get { return new Vector3(scale, scale, scale); } }

        public Vector3 Offset { get => offset; }
    }

    public enum Particles
    {
        Blood,
        BulletExplosion,
        TankRocketExplosion,
        RocketExplosion,
        TankExplosion,
        HelicopterExplosion,
        FireZone,
        Fire,
        ExplosionDecal,
        BloodPool,
        LevelUp,
        PlayerHit
    }
}