using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
	[Header("General")]
	[SerializeField] private Transform _container;
	[SerializeField] private int _repeatCount;
	[SerializeField] private int _distanceToFullLine;
	[SerializeField] private int _distanceToRandomLine;
	[SerializeField] private int _randomLinesBetweenFull;
	[SerializeField] private GameObject _finishLine;
	[SerializeField] private SnakeHead _head;

	private float blockDistance => _distanceToFullLine + _distanceToRandomLine * _randomLinesBetweenFull;

	[Header("Block")]
	[SerializeField] private Block _blockTemplate;
	[SerializeField] private int _blockSpawnChance;
	
	[Header("Wall")]
	[SerializeField] private Wall _wallTemplate;
	[SerializeField] private int _wallSpawnChance;

	[Header("Bonus")]
	[SerializeField] private Bonus _bonusTemplate;
	[SerializeField] private int _bonusSpawnChance;


	private BlockSpawnPoint[] _blockSpawnPoints;
	private WallSpawnPoint[] _wallSpawnPoints;
	private BonusSpawnPoint[] _bonusSpawnPoints;
	private int _blocksCreated = 0;

	public int BlocksCreated => _blocksCreated;

	public const float UNIT = 13.2f;

	private void Start()
	{
		_blockSpawnPoints = GetComponentsInChildren<BlockSpawnPoint>();
		_wallSpawnPoints = GetComponentsInChildren<WallSpawnPoint>();
		_bonusSpawnPoints = GetComponentsInChildren<BonusSpawnPoint>();

		GenerateBonusElements();
		SpawnBlock();
		_blocksCreated++;
	}

	private void Update()
	{
		if (_head.IsAlive && _blocksCreated < _repeatCount)
		{
			if (_head.transform.position.y >= transform.position.y - blockDistance)
			{
				SpawnBlock();
				_blocksCreated++;
			}
		}
		else
		{
			MoveSpawner(_distanceToRandomLine);
			GenerateElement(transform.localPosition, _finishLine);
			enabled = false;
		}
	}

	private void SpawnBlock()
	{
		MoveSpawner(_distanceToFullLine);
		GenerateFullLine(_blockSpawnPoints, _blockTemplate.gameObject);

		for (int i = 0; i < _randomLinesBetweenFull; i++)
		{

			GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance,  Random.Range(1, _distanceToRandomLine - 2));
			GenerateBonusElements();
			MoveSpawner(_distanceToRandomLine);
			GenerateRandomElements(_blockSpawnPoints, _blockTemplate.gameObject, _blockSpawnChance);

		}

		GenerateRandomElements(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance,  Random.Range(1, _distanceToRandomLine - 2));
		GenerateBonusElements();
	}


	private void GenerateBonusElements()
	{
		ShuffleYSpawnPoints(ref _bonusSpawnPoints);
		GenerateRandomElements(_bonusSpawnPoints, _bonusTemplate.gameObject, _bonusSpawnChance);
	}

	private void ShuffleYSpawnPoints(ref BonusSpawnPoint[] spawnPoints)
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			spawnPoints[i].transform.localPosition = new Vector3(spawnPoints[i].transform.localPosition.x,
																-Random.Range(1, _distanceToRandomLine),
																spawnPoints[i].transform.localPosition.z);
		}
	}

	private void GenerateFullLine(SpawnPoint[] spawnPoints, GameObject generatedElement)
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			GenerateElement(spawnPoints[i].transform.position, generatedElement);
		}
	}

	private void GenerateRandomElements(SpawnPoint[] spawnPoints, GameObject generatedElement, int spawnChance, int scaleY = 1)
	{
		 for (int i = 0; i < spawnPoints.Length; i++)
		{
			if (Random.Range(0,100) < spawnChance)
			{
				//Debug.Log(scaleY);
				GenerateElement(spawnPoints[i].transform.position, generatedElement, scaleY);
			}
		}
	}

	private void GenerateElement(Vector3 spawnPoint, GameObject generatedElement, float scaleY = 0)
	{
		GameObject element = Instantiate(generatedElement, spawnPoint, Quaternion.identity, _container);

		if(element.transform.childCount > 0)
		{
			GameObject child =  element.transform.GetChild(0).gameObject;
			if (child.TryGetComponent(out Stick stick))
			{
				ArrangeWall(stick, scaleY);
			}
		}
	}

	private void ArrangeWall(Stick stick, float scaleY)
	{
		float scaleSizeY = scaleY * UNIT;
		stick.transform.localScale = new Vector3(stick.transform.localScale.x,
												 scaleSizeY,
												 stick.transform.localScale.z);

		stick.transform.localPosition = new Vector3(stick.transform.localPosition.x,
													stick.transform.localPosition.y - scaleSizeY / 2.0f,
													stick.transform.localPosition.z);
	}

	private void MoveSpawner(int distanceY)
	{
		transform.position = new Vector3(transform.position.x,
										transform.position.y + distanceY,
										transform.position.z);
	}
}
