using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamagable
{
    public delegate void OnDeathEvent();
    public event OnDeathEvent OnDeath;


    [SerializeField]
    private float CurrentHealth;
    [SerializeField]
    private float TotalHealth;

    public float Health => CurrentHealth;
    public float MaxHealth => TotalHealth;



    protected virtual void Start()
    {
        CurrentHealth = MaxHealth;
    }

    internal void HealPlayer(int effect)
    {
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + effect, 0, MaxHealth);
        }

    }

    public virtual  void Destroy()
    {

        OnDeath?.Invoke();
        //Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Destroy();
        }
    }

    internal void SetCurrentHealth(float health)
    {
        CurrentHealth = health;
    }


}
