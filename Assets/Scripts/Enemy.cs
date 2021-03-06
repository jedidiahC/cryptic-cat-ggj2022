using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private RuneWords _counterType;
    [SerializeField] private string _word = "death";
    [SerializeField] private GameObject _segmentPrefab = null;
    [SerializeField] private EnemyMovement _enemyMovement = null;
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private Color _trailColor = Color.white;
    [SerializeField] private float _deathDelay = 0.05f;
    [SerializeField] private float _segDeathDelay = 0.2f;
    [SerializeField] private bool _initOnStart = false;

    private bool _isDead = false;
    private int _currSegDestroyed = -1;
    private EnemySegment[] _segments;
    private float _timer = 0;
    private float _health = 0;
    private int _wordLength = 0;

    public RuneWords CounterType { get { return _counterType; } }
    public bool IsDead { get { return _isDead; } }
    public int WordLength { get { return _wordLength; } }

    private void Awake()
    {
        Debug.Assert(_segmentPrefab != null, "_segmentPrefab is not assigned!");
        Debug.Assert(_enemyMovement != null, "_enemyMovement is not assigned!");
    }

    private void Start()
    {
        if (_initOnStart)
        {
            Init();
        }
    }

    private void Init()
    {
        SpawnSegments();
    }

    public void Init(string word, RuneWords counterType, EnemyPath enemyPath)
    {
        _word = word;
        _counterType = counterType;
        _wordLength = _word.Length;
        _health = _word.Length;

        _enemyMovement.SetPath(enemyPath);
        SpawnSegments();
    }

    private void SpawnSegments()
    {
        Transform[] segTransforms = new Transform[_word.Length];
        _segments = new EnemySegment[_word.Length];

        for (int c = 0; c < _word.Length; c++)
        {
            GameObject newSegGO = Instantiate(_segmentPrefab, transform.position, Quaternion.identity);
            Letter letter = newSegGO.GetComponent<Letter>();
            letter.SetText(_word[_word.Length - c - 1].ToString());
            segTransforms[c] = newSegGO.transform;
            _segments[c] = newSegGO.GetComponent<EnemySegment>();
            _segments[c].SetColor(_color);
            _segments[c].SetTrailColor(_trailColor);
            newSegGO.transform.SetParent(this.transform);
        }

        _enemyMovement.SetSegments(segTransforms);
    }

    public void Damage(float amount, RuneWords counterType)
    {
        if (_isDead) { return; }

        if (_counterType == counterType)
        {
            _health -= amount;
            if (_health <= 0)
            {
                OnDeath();
            }
        }
    }

    public void OnDeath()
    {
        // Debug.Log("On Countered");
        _isDead = true;
        // _enemyMovement.SetIsUpdating(false);
        _currSegDestroyed = 0;
    }

    private void Update()
    {
        if (_isDead)
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        if (_currSegDestroyed < 0 || _currSegDestroyed >= _segments.Length)
        {
            Destroy(this.gameObject, _deathDelay);
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            _segments[_currSegDestroyed].OnDeath();
            _currSegDestroyed++;
            _timer = _segDeathDelay;
        }
    }
}
