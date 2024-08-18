using UnityEngine;

public class DetectionScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Scripted Fall
            ScriptedFall scriptedFall = FindObjectOfType<ScriptedFall>();

            if (scriptedFall != null)
            {
                scriptedFall.CheckDetection();
            }

            //Scripted Fall 1
            ScriptedFall1 scriptedFall1 = FindObjectOfType<ScriptedFall1>();

            if (scriptedFall1 != null)
            {
                scriptedFall1.CheckDetection();
            }

            // Trigger CheckDetection for all ScriptedFall3 scripts in the scene
            Scripted_Fall3[] scriptedFall3Array = FindObjectsOfType<Scripted_Fall3>();

            foreach (Scripted_Fall3 scriptedFall3 in scriptedFall3Array)
            {
                if (scriptedFall3 != null)
                {
                    scriptedFall3.CheckDetection();
                }
            }

            //Spawn Debree
            DebreeSpawner debreeSpawner = FindObjectOfType<DebreeSpawner>();

            if (debreeSpawner != null)
            {
                debreeSpawner.SpawnCoroutine();
            }
        }

    }
}
