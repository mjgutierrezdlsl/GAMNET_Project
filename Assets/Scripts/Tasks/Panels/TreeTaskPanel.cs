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
    [SerializeField] private Image _treeImage;
    [SerializeField] private Sprite _idle, _hitLeft, _hitRight;

    private float _currentHealth;

    private void OnEnable()
    {
        ResetTask();
    }

    protected override void ResetTask()
    {
        base.ResetTask();
        _currentHealth = _maxHealth;
        _healthFill.fillAmount = _currentHealth / _maxHealth;
    }

    public void DamageTree()
    {
        StartCoroutine(HitTree());
        _currentHealth -= _damage;
        _healthFill.fillAmount = _currentHealth / _maxHealth;
        if (_currentHealth <= 0f)
        {
            CompleteTask();
        }
    }

    private IEnumerator HitTree()
    {
        var rand = Random.Range(0f, 1f);
        var hitDir = rand < 0.5f ? _hitLeft : _hitRight;
        _treeImage.sprite = hitDir;
        yield return new WaitForSeconds(0.1f);
        _treeImage.sprite = _idle;
    }
}
