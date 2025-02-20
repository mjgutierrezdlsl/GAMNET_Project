using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TreeTaskPanel : TaskPanel
{
    [Header("Health & Damage")]
    [SerializeField] private Image _healthFill;
    [SerializeField] private float _maxHealth = 10f;
    [SerializeField] private float _damage = 1f;

    [Header("Tree Animation")]
    [SerializeField] private Animator _treeAnimator;

    private float _currentHealth;


    private void OnEnable()
    {
        _currentHealth = _maxHealth;
        _healthFill.fillAmount = _currentHealth / _maxHealth;
    }

    public void DamageTree()
    {
        var rand = Random.Range(0f, 1f);
        var hitDir = rand > 0.5f ? "hitRight" : "hitLeft";
        _treeAnimator.SetTrigger(hitDir);
        _currentHealth -= _damage;
        _healthFill.fillAmount = _currentHealth / _maxHealth;
        if (_currentHealth <= 0f)
        {
            CompleteTask();
        }
    }
}
