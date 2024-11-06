using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image buffImage;     
	
    public Image debuffImage;

    [SerializeField]
	private SoundManager _soundManager;

    [SerializeField]
	private GameObject _playerPre;


    [SerializeField]
	private GameObject _cmsPre;

	[SerializeField]
	private GameObject _itemPrefab;

	[SerializeField]
	private float _timeMinCMS;

	[SerializeField]
	private float _timeMaxCMS;

	[SerializeField]
	private Text _scoreUI;

	private float _score;

	private GameObject _playerInstance;

	[SerializeField]
	private float _scoreIncreaseInterval = 0.1f;

	private float _scoreTimer;

	[SerializeField]
	private Vector3 _spawnPositionCMS;

	[SerializeField]
	private Vector3 _spawnPositionItem;

	[SerializeField]
	private float _spawnIntervalItem;

	[SerializeField]
	private Button _startButton;

	[SerializeField]
	private Button _replayButton;

	[SerializeField]
	private Text _gameOverText;

    [SerializeField]
    private Text _text1;
    [SerializeField]
    private Text _text2;

    [SerializeField]
	private AudioSource _backgroundMusic;

	[SerializeField]
	private AudioSource _soundEffectSource;

	[SerializeField]
	private AudioClip _correctAnswerSound;

	[SerializeField]
	private AudioClip _incorrectAnswerSound;

	private Coroutine _spawnCMSCoroutine;

	private Coroutine _spawnItemCoroutine;

	[SerializeField]
	private QuestionPanel questionPanel;

    private List<(string, string[], int)> questions = new List<(string, string[], int)>
    {
        ("Câu Hỏi: Độ khó trong trò chơi nên như thế nào để người chơi cảm thấy có sự tiến bộ?", new string[] { "Giảm dần", "Tăng dần", "Không đổi", "Ngẫu nhiên" }, 1),
        ("Câu Hỏi: Game Design Document (GDD) là gì?", new string[] {
            "Tài liệu mô tả cách lập trình trò chơi", "Tài liệu mô tả chi tiết tất cả các khía cạnh của trò chơi", "Tài liệu chỉ tập trung vào thiết kế nhân vật", "Tài liệu chỉ tập trung vào cốt truyện" }, 1),
        ("Câu Hỏi: Trong một game 2D, trục tọa độ nào thường được dùng để biểu diễn chiều ngang (chiều rộng) của đối tượng?", new string[] { "X", "Y", "Z", "W" }, 0),
        ("Câu Hỏi: Khi phát triển một trò chơi, yếu tố nào được coi là quan trọng nhất để thu hút người chơi?", new string[] { "Đồ họa đẹp", "Đồ họa đẹp", "Gameplay thú vị", "Âm thanh hoành tráng"}, 2 ),
        ("Câu Hỏi: Trong lập trình game, thuật ngữ \"FPS\" thường được sử dụng để chỉ điều gì?", new string[] { "First Person Shooter", "Frames Per Second", "Fast Processing Speed", "Forward Player Support"}, 1 ),
        ("Câu Hỏi: Phần mềm nào sau đây không phải là công cụ lập trình game?", new string[] { "Unity", "Unreal Engine", "Godot", "Excel"}, 3 ),
        ("Câu Hỏi: Đâu là ngôn ngữ lập trình phổ biến nhất khi phát triển game trên Unity?", new string[] { "Python", "Java", "C#","C++"}, 2 ),
        ("Câu Hỏi: Thành phần nào của game chịu trách nhiệm tính toán và xử lý tương tác giữa các đối tượng?", new string[] { "Engine vật lý", "Đồ họa", "Âm thanh", "Giao diện người dùng"}, 0 ),
        ("Câu Hỏi: Nếu một game bị \"lag\" thường là do yếu tố nào gây ra?", new string[] { "Quá tải CPU", "Card đồ họa không đủ mạnh", "Bộ nhớ RAM không đủ", "Tất cả đều đúng"}, 3 ),
        ("Câu Hỏi: Để tạo ra chuyển động mượt mà của nhân vật trong game, lập trình viên thường sử dụng gì?", new string[] { "Các hình ảnh động (Animations)", "Tăng kích thước đối tượng", "Các vòng lặp vô hạn", "Tắt đồ họa"}, 0 ),
        ("Câu Hỏi: Trong một trò chơi, yếu tố nào quan trọng nhất để giữ chân người chơi lâu dài?", new string[] { "Độ khó của game", "Tính tương tác và cốt truyện", "Đồ họa chân thực", "Hướng dẫn chơi chi tiết"}, 1 ),
        ("Câu Hỏi: Trong Unity, chức năng nào giúp lập trình viên kiểm tra và theo dõi lỗi?", new string[] { "Console", "Inspector", "Asset Store", "Scene"}, 0 ),
        ("Câu Hỏi: Trong Unity, chức năng \"Animator\" chủ yếu dùng để làm gì?", new string[] { "Tạo môi trường 3D", "Tạo hoạt ảnh chuyển động cho đối tượng", "Thiết lập âm thanh cho game", "Quản lý ánh sáng trong game"}, 1 ),
        ("Câu Hỏi: Trong Unity, thành phần nào dùng để gắn script vào đối tượng?", new string[] { "Rigidbody", "Transform", "Component", "ScriptableObject"}, 2 ),
        ("Câu Hỏi: Trong lập trình game, đối tượng \"sprite\" chủ yếu được dùng trong game nào?", new string[] { "Game thực tế ảo", "Game mô phỏng", "Game 3D", "Game 2D"}, 3 ),
        ("Câu Hỏi: Trong Unity, \"Collider\" dùng để làm gì?", new string[] { "Tạo âm thanh trong game", "Điều khiển tốc độ của đối tượng", "Thiết lập vùng tương tác và va chạm của đối tượng", "Thay đổi màu sắc của đối tượng"}, 2 ),
        ("Câu Hỏi: Trong game 3D, trục Z thường biểu thị chiều nào?", new string[] { "Chiều ngang", "Chiều cao", "Chiều sâu", "Chiều sáng"}, 2 ),
        ("Câu Hỏi: Trong Unity, \"GameObject\" là gì?", new string[] { "Một thành phần ánh sáng", "Một thành phần âm thanh", "Một đối tượng điều khiển người chơi", "Một đối tượng cơ bản có thể chứa các thành phần khác"}, 3 ),
        ("Câu Hỏi: Trong lập trình game 2D, yếu tố nào quan trọng nhất để tạo nên một hệ thống va chạm chính xác?", new string[] { "Collider phù hợp cho từng đối tượng", "Tốc độ khung hình cao", "Tạo thêm đối tượng va chạm ảo", "Kích thước sprite chuẩn xác"}, 0 ),
        ("Câu Hỏi: Trong Unity, \"Canvas\" được sử dụng trong trường hợp nào?", new string[] { "Để thiết lập ánh sáng trong game", "Để tạo giao diện người dùng (UI)", "Để điều khiển nhân vật trong game", "Để lưu trữ dữ liệu của game"}, 1 ),
        ("Câu Hỏi (Đố Vui): Nếu một game thủ đi mua sắm, họ sẽ mua gì đầu tiên?", new string[] { "Đồ ăn", "Trang phục mới", "Nước uống", "Phụ kiện gaming"}, 3 ),
        ("Câu Hỏi (Đố Vui): Nếu một game có nhân vật chính là một cái bàn, game đó sẽ tên là gì?", new string[] { "Bàn tay không", "Đừng có làm bàn!", "Bàn chân không", "Bàn thắng!"}, 1 ),
        ("Câu Hỏi (Đố Vui): Nếu lập trình viên game chơi trò chơi đố vui, họ sẽ thắng như thế nào?", new string[] {"A", "B","C","D"}, 3 ),
        ("Câu Hỏi (Đố Mẹo): Tôi là một dòng mã mà mọi lập trình viên đều ghét, tôi làm cho game không chạy, tôi là gì?", new string[] { "Lỗi (bug)", "Đoạn mã rỗng", "Biến không được khai báo", "Cấu trúc điều kiện"}, 0 ),
        ("Câu Hỏi (Đố Mẹo): Cái gì có thể \"chạy\" nhưng không bao giờ \"đi\"?", new string[] { "Một đoạn mã", "Một con robot", "Một game", "Một chiếc ô tô"}, 2 ),
        ("Câu Hỏi (Đố Mẹo): Cái gì có thể khiến game thủ cười nhưng cũng khiến họ khóc?", new string[] { "Game hay", "Game lỗi", "Game có nội dung buồn", "Game quá khó"}, 1 ),
        ("Câu Hỏi: Lập trình game là gì?", new string[] { "Một quá trình phát triển phần mềm tương tác và giải trí", "Một quá trình tạo ra các trang web", "Một phương pháp dạy học", "Một loại nghệ thuật thị giác"}, 0 ),
        ("Câu Hỏi: Khác biệt giữa game 2D và 3D là gì?", new string[] { "Game 2D chỉ có thể chơi trên máy tính", "Game 3D không có nhân vật", "Game 2D không có âm thanh", "Game 3D có chiều sâu, trong khi game 2D chỉ có chiều rộng và chiều cao"}, 3 ),
        ("Câu Hỏi: Công việc của nhà thiết kế game (Game Designer) thường bao gồm gì?", new string[] { "Tạo ra cốt truyện, cơ chế và thiết kế cấp độ cho game", "Lập trình mã nguồn cho game", "Quản lý dự án phát triển game", "Chỉ tập trung vào âm thanh"}, 0 ),
        ("Câu Hỏi: Game Tester chịu trách nhiệm về điều gì trong quá trình phát triển game?", new string[] { "Tạo ra thiết kế game", "Viết mã lập trình", "Kiểm tra lỗi, đảm bảo chất lượng và báo cáo các vấn đề", "Tạo hình ảnh và đồ họa cho game"}, 2 ),
    };

    public static GameManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		_startButton.onClick.AddListener(StartGame);
		_replayButton.onClick.AddListener(ReplayGame);
		_replayButton.gameObject.SetActive(value: false);
		_gameOverText.gameObject.SetActive(value: false);
        _text1.gameObject.SetActive(value: false);
        _text2.gameObject.SetActive(value: false);
        Time.timeScale = 0f;
		_backgroundMusic.volume = 0.2f;
		_soundEffectSource.volume = 1f;
		_backgroundMusic.Play();
        CleanupScene();
        _soundManager.StartCoroutine("PlayLoopingSounds");
    }

	private void Update()
	{
		if (_playerInstance != null)
		{
			if (_playerInstance.GetComponent<Player>().IsDead())
			{
				EndGame();
			}
			else
			{
				UpdateScore();
			}
		}
	}

	private void StartGame()
	{
		CleanupScene();
		_playerInstance = Object.Instantiate(_playerPre, new Vector3(-4.3f, -1.5f, -1f), Quaternion.identity);
        _score = 0f;
		_scoreUI.text = "Score: " + _score;
		_startButton.gameObject.SetActive(value: false);
		_replayButton.gameObject.SetActive(value: false);
		_gameOverText.gameObject.SetActive(value: false);
        _text1.gameObject.SetActive(false);
        _text2.gameObject.SetActive(false);
        Time.timeScale = 1f;
		StopAllCoroutines();
		_spawnCMSCoroutine = StartCoroutine(SpawnCMSCoroutine());
		_spawnItemCoroutine = StartCoroutine(SpawnItemCoroutine());
        _soundManager.StartCoroutine("PlayLoopingSounds");
    }

	private void ReplayGame()
	{
		EndGame();
		StartGame();
	}

	private void EndGame()
	{
		Time.timeScale = 0f;
		_gameOverText.gameObject.SetActive(value: true);
		_replayButton.gameObject.SetActive(value: true);
        if (_score < 0)
        {
            _text1.gameObject.SetActive(true);
            _text2.gameObject.SetActive(false);
        }
        else
        {
            _text2.gameObject.SetActive(true);
            _text1.gameObject.SetActive(false);
        }
        StopAllCoroutines();
    }

	private void UpdateScore()
	{
		_scoreTimer += Time.deltaTime;
		if (_scoreTimer >= _scoreIncreaseInterval)
		{
			_score += 1f;
			_scoreUI.text = "Score: " + _score;
			_scoreTimer = 0f;
		}
	}

	private void CleanupScene()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("CMS");
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i]);
		}
		array = GameObject.FindGameObjectsWithTag("Item");
		for (int i = 0; i < array.Length; i++)
		{
			Object.Destroy(array[i]);
		}
		if (_playerInstance != null)
		{
			Object.Destroy(_playerInstance);
		}
	}

	public void OnItemCollected()
	{
		(string, string[], int) tuple = questions[Random.Range(0, questions.Count)];
		questionPanel.ShowQuestion(tuple.Item1, tuple.Item2, tuple.Item3, OnQuestionAnswered);
		Time.timeScale = 0f;
	}

	private void OnQuestionAnswered(bool isCorrect)
	{
		Time.timeScale = 1f;
		if (isCorrect)
		{
			ApplyBuff();
			_soundEffectSource.PlayOneShot(_correctAnswerSound);
		}
		else
		{
			ApplyDebuff();
			_soundEffectSource.PlayOneShot(_incorrectAnswerSound);
		}
	}

	private void ApplyBuff()
	{
		Debug.Log("bạn đã trả lời đúng!");
        _score += 2000;
        _scoreUI.text = "Điểm: " + _score.ToString();
        StartCoroutine(ShowTemporaryImage(buffImage));
    }

	private void ApplyDebuff()
	{
		Debug.Log("bạn đã trả lời sai!");
		_score -= 4000;
		_scoreUI.text = "Điểm: " + _score.ToString();
        StartCoroutine(ShowTemporaryImage(debuffImage));
    }

	private IEnumerator SpawnCMSCoroutine()
	{
		while (true)
		{
			Object.Instantiate(_cmsPre, _spawnPositionCMS, Quaternion.identity);
			float seconds = Random.Range(_timeMinCMS, _timeMaxCMS);
			yield return new WaitForSeconds(seconds);
		}
	}

    private IEnumerator ShowTemporaryImage(Image image)
    {
        image.gameObject.SetActive(true); 
        yield return new WaitForSeconds(0.5f); 
        image.gameObject.SetActive(false);
    }
    private IEnumerator SpawnItemCoroutine()
	{
		while (true)
		{
			Object.Instantiate(_itemPrefab, _spawnPositionItem, Quaternion.identity);
			yield return new WaitForSeconds(_spawnIntervalItem);
		}
	}
}
