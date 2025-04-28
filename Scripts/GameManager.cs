using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI questionText, scoreText, timerText, feedbackText, levelIndicatorText;
    public Button[] answerButtons;
    public Button powerUpButton;
    public TMP_InputField wordInput;
    public Button submitWordButton;
    public Slider progressBar;
    public LearnMore learnMore;
    public ParticleSystem confettiEffect;
    public Button nextButton;
    public GameObject leaderboardPanel; 
    public TextMeshProUGUI leaderboardText; 

    private List<Question> questions;
    private int gameMode;
    private int currentQuestionIndex = 0;
    private int score = 0;
    private int highScore;
    private float timeLeft = 15f;
    private bool isAnswering = false;
    private int powerUpCount = 0;
    private int correctStreak = 0;
    private bool waitingForNextQuestion = false;
    private string playerNickname;

    private readonly Color[] modeColors = new Color[]
    {
        new Color(0f, 120f/255f, 1f), 
        new Color(0f, 1f, 0f),        
        new Color(1f, 0f, 0f)         
    };

    void Start()
    {
        // Ensure this script only runs in the QuizGame scene
        if (SceneManager.GetActiveScene().name != "QuizGame")
        {
            gameObject.SetActive(false);
            return;
        }

        playerNickname = PlayerPrefs.GetString("PlayerNickname", "Player");
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        gameMode = PlayerPrefs.GetInt("GameMode", 1);
        LoadQuestions();
        StartGameMode();
        UpdateUI();

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
        if (learnMore != null && learnMore.explanationText != null)
            learnMore.explanationText.gameObject.SetActive(false);
        if (leaderboardPanel != null)
            leaderboardPanel.SetActive(false);
    }

    void LoadQuestions()
    {
        var level1Questions = new List<Question>
        {
            new Question("What is a variable in programming?", new string[] {"A loop structure", "A named storage location", "A type of function", "A sorting algorithm"}, 1, QuestionType.MultipleChoice),
            new Question("What does OOP stand for?", new string[] {"Object-Oriented Programming", "Operational Output Processing", "Organized Operator Protocol", "Open Optimization Platform"}, 0, QuestionType.MultipleChoice),
            new Question("What is a loop used for?", new string[] {"Storing data", "Repeating code", "Defining functions", "Sorting arrays"}, 1, QuestionType.MultipleChoice),
            new Question("What is a function?", new string[] {"A data type", "A reusable block of code", "A conditional statement", "A variable"}, 1, QuestionType.MultipleChoice),
            new Question("What does 'if' statement do?", new string[] {"Loops code", "Defines a class", "Checks a condition", "Prints output"}, 2, QuestionType.MultipleChoice),
            new Question("What is inheritance in OOP?", new string[] {"Creating loops", "Reusing code from a parent class", "Defining variables", "Sorting data"}, 1, QuestionType.MultipleChoice),
            new Question("What is a boolean?", new string[] {"A loop type", "A data type with true/false", "A function", "A class"}, 1, QuestionType.MultipleChoice),
            new Question("What is encapsulation?", new string[] {"Hiding data", "Sorting data", "Looping code", "Defining arrays"}, 0, QuestionType.MultipleChoice),
            new Question("What is a compiler?", new string[] {"A loop", "Translates code to machine language", "A variable", "A function"}, 1, QuestionType.MultipleChoice),
            new Question("What is a string?", new string[] {"A number", "A sequence of characters", "A loop", "A class"}, 1, QuestionType.MultipleChoice)
        };
        var level2Questions = new List<Question>
        {
            new Question("Bubble Sort is efficient for large datasets.", new string[] {"True", "False"}, 1, QuestionType.TrueFalse),
            new Question("Binary Search requires a sorted array.", new string[] {"True", "False"}, 0, QuestionType.TrueFalse),
            new Question("Recursion is when a function calls itself.", new string[] {"True", "False"}, 0, QuestionType.TrueFalse),
            new Question("Quick Sort has O(n^2) worst-case complexity.", new string[] {"True", "False"}, 0, QuestionType.TrueFalse),
            new Question("Linear Search is faster than Binary Search.", new string[] {"True", "False"}, 1, QuestionType.TrueFalse),
            new Question("Merge Sort divides the array into halves.", new string[] {"True", "False"}, 0, QuestionType.TrueFalse),
            new Question("Dijkstra’s algorithm finds the shortest path.", new string[] {"True", "False"}, 0, QuestionType.TrueFalse),
            new Question("Selection Sort swaps the minimum element.", new string[] {"True", "False"}, 0, QuestionType.TrueFalse),
            new Question("Depth-First Search uses a stack.", new string[] {"True", "False"}, 0, QuestionType.TrueFalse),
            new Question("Breadth-First Search uses a queue.", new string[] {"True", "False"}, 0, QuestionType.TrueFalse)
        };
        var level3Questions = new List<Question>
        {
            new Question("Rearrange 'KCATSS' to form a data structure.", new string[] {"STACK"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'UEUQE' to form a data structure.", new string[] {"QUEUE"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'EERT' to form a data structure.", new string[] {"TREE"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'TSIL' to form a data structure.", new string[] {"LIST"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'YARRA' to form a data structure.", new string[] {"ARRAY"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'HPARG' to form a data structure.", new string[] {"GRAPH"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'EAPH' to form a data structure.", new string[] {"HEAP"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'KEDNIL' to form a data structure.", new string[] {"LINKED"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'EUNOHSAH' to form a data structure.", new string[] {"HASH"}, 0, QuestionType.WordGame),
            new Question("Rearrange 'EUQED' to form a data structure.", new string[] {"DEQUE"}, 0, QuestionType.WordGame)
        };

        questions = gameMode switch
        {
            1 => level1Questions,
            2 => level2Questions,
            3 => level3Questions,
            _ => level1Questions
        };
    }

    void StartGameMode()
    {
        currentQuestionIndex = 0;
        if (progressBar != null)
        {
            progressBar.maxValue = 10;
            progressBar.value = 0;
        }
        UpdateLevelIndicator();
        DisplayQuestion();
        StartCoroutine(TimerCoroutine());
    }

    void UpdateLevelIndicator()
    {
        if (levelIndicatorText == null)
        {
            Debug.LogWarning("LevelIndicatorText is not assigned in GameManager.");
            return;
        }

        string modeName = gameMode switch
        {
            1 => "Programming Concepts",
            2 => "Algorithms",
            3 => "Data Structures",
            _ => "Unknown Mode"
        };
        levelIndicatorText.text = $"Mode: {modeName}";
        levelIndicatorText.color = modeColors[gameMode - 1];

        if (gameMode <= 2)
        {
            foreach (var button in answerButtons)
            {
                if (button != null && button.gameObject.activeSelf)
                {
                    button.GetComponent<Image>().color = modeColors[gameMode - 1];
                }
            }
        }
    }

    void DisplayQuestion()
    {
        Question question = GetCurrentQuestion();
        if (questionText != null)
            questionText.text = question.text;
        else
            Debug.LogWarning("QuestionText is not assigned in GameManager.");

        isAnswering = true;
        timeLeft = 15f;

        if (gameMode == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                if (answerButtons[i] != null)
                {
                    answerButtons[i].gameObject.SetActive(true);
                    var buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                        buttonText.text = question.options[i];
                    else
                        Debug.LogWarning($"AnswerButton{i} is missing a TextMeshProUGUI component.");
                    int index = i;
                    answerButtons[i].onClick.RemoveAllListeners();
                    answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
                }
                else
                {
                    Debug.LogWarning($"AnswerButton{i} is not assigned in GameManager.");
                }
            }
            if (wordInput != null)
                wordInput.gameObject.SetActive(false);
            if (submitWordButton != null)
                submitWordButton.gameObject.SetActive(false);
        }
        else if (gameMode == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                if (answerButtons[i] != null)
                {
                    answerButtons[i].gameObject.SetActive(true);
                    var buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                        buttonText.text = question.options[i];
                    else
                        Debug.LogWarning($"AnswerButton{i} is missing a TextMeshProUGUI component.");
                    int index = i;
                    answerButtons[i].onClick.RemoveAllListeners();
                    answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
                }
                else
                {
                    Debug.LogWarning($"AnswerButton{i} is not assigned in GameManager.");
                }
            }
            for (int i = 2; i < 4; i++)
                if (answerButtons[i] != null)
                    answerButtons[i].gameObject.SetActive(false);
            if (wordInput != null)
                wordInput.gameObject.SetActive(false);
            if (submitWordButton != null)
                submitWordButton.gameObject.SetActive(false);
        }
        else if (gameMode == 3)
        {
            for (int i = 0; i < 4; i++)
                if (answerButtons[i] != null)
                    answerButtons[i].gameObject.SetActive(false);
            if (wordInput != null)
            {
                wordInput.gameObject.SetActive(true);
                wordInput.text = "";
                StartCoroutine(FocusInputField());
            }
            if (submitWordButton != null)
            {
                submitWordButton.gameObject.SetActive(true);
                submitWordButton.onClick.RemoveAllListeners();
                submitWordButton.onClick.AddListener(() => CheckAnswer(0));
            }
        }

        if (powerUpButton != null)
        {
            powerUpButton.gameObject.SetActive(powerUpCount > 0 && (gameMode == 1 || gameMode == 3));
            powerUpButton.onClick.RemoveAllListeners();
            powerUpButton.onClick.AddListener(UsePowerUp);
            var powerUpText = powerUpButton.GetComponentInChildren<TextMeshProUGUI>();
            if (powerUpText != null)
                powerUpText.text = gameMode == 1 ? "50/50" : "Reveal Letter";
        }

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
        if (learnMore != null)
            learnMore.gameObject.SetActive(false);
        if (learnMore != null && learnMore.explanationText != null)
            learnMore.explanationText.gameObject.SetActive(false);
    }

    private IEnumerator FocusInputField()
    {
        yield return new WaitForEndOfFrame();
        if (wordInput != null)
        {
            wordInput.ActivateInputField();
            wordInput.Select();
        }
    }

    void CheckAnswer(int selectedIndex)
    {
        if (!isAnswering) return;
        isAnswering = false;
        Question question = GetCurrentQuestion();
        bool isCorrect = false;
        string key;

        if (gameMode == 3)
        {
            string userAnswer = wordInput != null ? wordInput.text.ToUpper().Trim() : "";
            Debug.Log($"User Answer in Mode 3: {userAnswer}");
            isCorrect = userAnswer == question.options[0];
            key = question.options[0]; // Correct answer for Mode 3
        }
        else if (gameMode == 2)
        {
            isCorrect = selectedIndex == question.correctIndex;
            key = question.text; // Question text for Mode 2
        }
        else
        {
            isCorrect = selectedIndex == question.correctIndex;
            key = question.options[question.correctIndex]; // Correct answer for Mode 1
        }

        if (isCorrect)
        {
            int points = Mathf.FloorToInt(100 * (timeLeft / 15f));
            score += points;
            if (feedbackText != null)
            {
                feedbackText.text = "Correct!";
                feedbackText.GetComponent<Animator>().SetTrigger("ShowFeedback");
            }
            if (confettiEffect != null)
                confettiEffect.Play();
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayCorrectSound();
            correctStreak++;
            if (correctStreak >= 3)
            {
                powerUpCount++;
                correctStreak = 0;
            }
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.text = "Try Again!";
                feedbackText.GetComponent<Animator>().SetTrigger("ShowFeedback");
            }
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayIncorrectSound();
            correctStreak = 0;
        }

        if (learnMore != null)
        {
            Debug.Log($"Preparing explanation for key: {key} in Mode {gameMode}");
            learnMore.PrepareExplanation(key, gameMode);
        }
        UpdateUI();
        waitingForNextQuestion = true;
        if (nextButton != null)
            nextButton.gameObject.SetActive(true);
    }

    public void NextQuestion()
    {
        if (!waitingForNextQuestion) return;
        waitingForNextQuestion = false;

        currentQuestionIndex++;
        if (progressBar != null)
            progressBar.value = currentQuestionIndex;

        if (currentQuestionIndex >= 10)
        {
            EndGame();
            return;
        }

        DisplayQuestion();
        StartCoroutine(TimerCoroutine());
    }

    void EndGame()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Update the leaderboard
        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.UpdateLeaderboard(playerNickname, score);
        }

        // Display the leaderboard
        if (leaderboardPanel != null && leaderboardText != null)
        {
            leaderboardPanel.SetActive(true);
            var entries = LeaderboardManager.Instance.GetLeaderboard();
            string leaderboardDisplay = "Leaderboard\n\n";
            for (int i = 0; i < entries.Count; i++)
            {
                leaderboardDisplay += $"{i + 1}. {entries[i].nickname}: {entries[i].score}\n";
            }
            leaderboardText.text = leaderboardDisplay;
        }
        else
        {
            if (questionText != null)
                questionText.text = $"Game Over! Final Score: {score}\nHigh Score: {highScore}";
        }

        if (levelIndicatorText != null)
            levelIndicatorText.text = "";
        for (int i = 0; i < 4; i++)
            if (answerButtons[i] != null)
                answerButtons[i].gameObject.SetActive(false);
        if (wordInput != null)
            wordInput.gameObject.SetActive(false);
        if (submitWordButton != null)
            submitWordButton.gameObject.SetActive(false);
        if (powerUpButton != null)
            powerUpButton.gameObject.SetActive(false);
        if (learnMore != null)
            learnMore.gameObject.SetActive(false);
        if (learnMore != null && learnMore.explanationText != null)
            learnMore.explanationText.gameObject.SetActive(false);
        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
        StartCoroutine(ReturnToMainMenu());
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }

    void UsePowerUp()
    {
        if (powerUpCount <= 0) return;
        powerUpCount--;

        if (gameMode == 1)
        {
            Question question = GetCurrentQuestion();
            List<int> incorrectIndices = new List<int>();
            for (int i = 0; i < 4; i++)
                if (i != question.correctIndex)
                    incorrectIndices.Add(i);
            incorrectIndices.Shuffle();
            if (answerButtons[incorrectIndices[0]] != null)
                answerButtons[incorrectIndices[0]].gameObject.SetActive(false);
            if (answerButtons[incorrectIndices[1]] != null)
                answerButtons[incorrectIndices[1]].gameObject.SetActive(false);
        }
        else if (gameMode == 3)
        {
            Question question = GetCurrentQuestion();
            string answer = question.options[0];
            int revealIndex = Random.Range(0, answer.Length);
            string currentInput = wordInput != null ? wordInput.text.PadRight(answer.Length, '_') : new string('_', answer.Length);
            char[] chars = currentInput.ToCharArray();
            chars[revealIndex] = answer[revealIndex];
            if (wordInput != null)
                wordInput.text = new string(chars);
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPowerUpSound();
        UpdateUI();
    }

    IEnumerator TimerCoroutine()
    {
        while (timeLeft > 0 && isAnswering)
        {
            timeLeft -= Time.deltaTime;
            if (timerText != null)
                timerText.text = $"Time: {Mathf.CeilToInt(timeLeft)}";
            yield return null;
        }
        if (isAnswering)
        {
            isAnswering = false;
            if (feedbackText != null)
            {
                feedbackText.text = "Time's Up!";
                feedbackText.GetComponent<Animator>().SetTrigger("ShowFeedback");
            }
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayIncorrectSound();
            if (learnMore != null)
            {
                Question question = GetCurrentQuestion();
                string key = gameMode == 2 ? question.text : question.options[question.correctIndex];
                Debug.Log($"Preparing explanation for key: {key} in Mode {gameMode}");
                learnMore.PrepareExplanation(key, gameMode);
            }
            waitingForNextQuestion = true;
            if (nextButton != null)
                nextButton.gameObject.SetActive(true);
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
        if (powerUpCount > 0 && (gameMode == 1 || gameMode == 3) && powerUpButton != null)
        {
            var powerUpText = powerUpButton.GetComponentInChildren<TextMeshProUGUI>();
            if (powerUpText != null)
                powerUpText.text = gameMode == 1 ? "50/50" : "Reveal Letter";
        }
    }

    Question GetCurrentQuestion()
    {
        return questions[currentQuestionIndex];
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}