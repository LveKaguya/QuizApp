using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LearnMore : MonoBehaviour
{
    public TextMeshProUGUI explanationText;
    private string lastKey;
    private int lastGameMode;
    private string lastExplanation;

    private Dictionary<string, string> explanationsMode1 = new Dictionary<string, string>
    {
        { "A named storage location", "A variable is a named storage location in memory that holds a value, which can be changed during program execution." },
        { "Object-Oriented Programming", "OOP stands for Object-Oriented Programming, a paradigm based on objects and classes." },
        { "Repeating code", "A loop is used to repeat a block of code multiple times until a condition is met." },
        { "A reusable block of code", "A function is a reusable block of code that performs a specific task and can return a value." },
        { "Checks a condition", "An 'if' statement checks a condition and executes code if the condition is true." },
        { "Reusing code from a parent class", "Inheritance in OOP allows a class to inherit properties and methods from a parent class." },
        { "A data type with true/false", "A boolean is a data type that can only be true or false, often used in conditions." },
        { "Hiding data", "Encapsulation is the concept of hiding data and methods within a class, exposing only what is necessary." },
        { "Translates code to machine language", "A compiler translates high-level code into machine language that a computer can execute." },
        { "A sequence of characters", "A string is a sequence of characters, used to represent text in programming." }
    };

    private Dictionary<string, string> explanationsMode2 = new Dictionary<string, string>
    {
        { "Bubble Sort is efficient for large datasets.", "Bubble Sort is inefficient for large datasets due to its O(n^2) time complexity." },
        { "Binary Search requires a sorted array.", "Binary Search requires a sorted array to work efficiently, with O(log n) time complexity." },
        { "Recursion is when a function calls itself.", "Recursion occurs when a function calls itself to solve a smaller instance of the same problem." },
        { "Quick Sort has O(n^2) worst-case complexity.", "Quick Sort has O(n^2) worst-case complexity but averages O(n log n) with good pivots." },
        { "Linear Search is faster than Binary Search.", "Linear Search is slower than Binary Search, with O(n) vs. O(log n) time complexity." },
        { "Merge Sort divides the array into halves.", "Merge Sort divides the array into halves, sorts them, and merges them back together." },
        { "Dijkstra’s algorithm finds the shortest path.", "Dijkstra’s algorithm finds the shortest path in a weighted graph with non-negative weights." },
        { "Selection Sort swaps the minimum element.", "Selection Sort repeatedly finds the minimum element and swaps it into the sorted portion." },
        { "Depth-First Search uses a stack.", "Depth-First Search (DFS) uses a stack (explicitly or via recursion) to explore nodes." },
        { "Breadth-First Search uses a queue.", "Breadth-First Search (BFS) uses a queue to explore nodes level by level." }
    };

    private Dictionary<string, string> explanationsMode3 = new Dictionary<string, string>
    {
        { "STACK", "STACK is a data structure that follows Last In, First Out (LIFO)." },
        { "QUEUE", "QUEUE is a data structure that follows First In, First Out (FIFO)." },
        { "TREE", "TREE is a hierarchical data structure with nodes and branches." },
        { "LIST", "LIST is a linear data structure where elements are stored sequentially." },
        { "ARRAY", "ARRAY is a data structure that stores elements of the same type in contiguous memory." },
        { "GRAPH", "GRAPH is a data structure with nodes connected by edges." },
        { "HEAP", "HEAP is a tree-based data structure that satisfies the heap property." },
        { "LINKED", "LINKED refers to a Linked List, where elements are nodes with pointers." },
        { "HASH", "HASH refers to a Hash Table, which maps keys to values for efficient lookup." },
        { "DEQUE", "DEQUE (Double-Ended Queue) allows insertion and deletion at both ends." }
    };

    void Start()
    {
        if (gameObject != null)
            gameObject.SetActive(false);
        if (explanationText != null)
            explanationText.gameObject.SetActive(false);
    }

    public void PrepareExplanation(string key, int gameMode)
    {
        if (explanationText == null)
        {
            Debug.LogWarning("ExplanationText is not assigned in LearnMore script.");
            return;
        }

        lastKey = key;
        lastGameMode = gameMode;

        Dictionary<string, string> explanations = gameMode switch
        {
            1 => explanationsMode1,
            2 => explanationsMode2,
            3 => explanationsMode3,
            _ => explanationsMode1
        };

        if (explanations.TryGetValue(key, out string explanation))
        {
            lastExplanation = explanation;
        }
        else
        {
            lastExplanation = "No explanation available.";
        }

        if (gameObject != null)
            gameObject.SetActive(true);
    }

    public void ShowExplanationWrapper()
    {
        if (explanationText == null)
        {
            Debug.LogWarning("ExplanationText is not assigned in LearnMore script.");
            return;
        }

        if (!string.IsNullOrEmpty(lastExplanation))
        {
            explanationText.text = lastExplanation;
            explanationText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No explanation available to show.");
        }
    }
}