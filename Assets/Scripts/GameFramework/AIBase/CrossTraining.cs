using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CrossTraining : TrainingRunner
{
    [SerializeField]
    AITrainer trainable;
    [SerializeField]
    AIController starting;
    [SerializeField]
    LoadedAI loadedAI;

    int tryCount = 3;
    int generationCount = 25;

    int sessionNumber = 0;

    string trained;

    private void Update()
    {
        if (sessionNumber > 3)
            return;

        if (!TrainingInProgess)
        {
            if (sessionNumber == 0)
            {
                StartTraining(trainable, starting, tryCount, generationCount);
                sessionNumber++;
            }                
            else if (sessionNumber == 1)
            {
                trained = FindNewestFile();
                StartTraining(starting, trainable, tryCount, generationCount);
                sessionNumber++;
            }
            else
            {
                
              

                if (sessionNumber % 2 == 0)
                    StartTraining(trainable, trainable, tryCount, generationCount, trained, null);
                else
                    StartTraining(trainable, trainable, tryCount, generationCount, null, trained);

                trained = FindNewestFile();
                sessionNumber++;
            }
        }
            
    }

    private string FindNewestFile()
    {
        string file = null;
        string directoryPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Trained", trainable.GetType().Name);

        if (Directory.Exists(directoryPath))
        {
            var directory = new DirectoryInfo(directoryPath);
            file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().Name;            
        }

        if (file == null)
            Debug.LogError("File not found");

        return file ;
    }
}
