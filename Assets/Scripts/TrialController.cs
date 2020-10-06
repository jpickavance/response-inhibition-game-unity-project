using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialController : MonoBehaviour
{
    public ExperimentController experimentController;
    public List<int> certainty = new List<int>();
    public List<int> stopTrials = new List<int>();
    // Create list of certainty trials and stop trials. Note, takes values from experimental controller: n_trials and n_bins 
        // n_trials must have n_bins as a factor
        // n_trials/(over)n_bins must also have 2*(times)the denominator of stop trials as a factor
        // This code is for 1/3 stop trials on uncertain conditions
    void Start()
    {
        double n_trials = experimentController.n_trials;
        int n_bins = experimentController.n_bins;
        for(int j = 0; j < experimentController.n_bins; j++)
        {
            List<int> tempListCertainty = new List<int>();
            List<int> tempListStop = new List<int>();
            for(int i = 0; i < n_trials/(2*n_bins); i++)
            {
                tempListCertainty.Add(0);
            }
            for(int i = 0; i < n_trials/(2*n_bins); i++)
            {
                tempListCertainty.Add(1);
            }
            for(int i = 0; i < n_trials/(6*n_bins); i++)
            {
                tempListStop.Add(1);
            }
            for(int i = 0; i < ((5*n_trials)/(6*n_bins)); i++)
            {
                tempListStop.Add(0);
            }
            Shuffle(tempListCertainty, j);
            Shuffle(tempListStop, j);
            certainty.AddRange(tempListCertainty);
            stopTrials.AddRange(tempListStop);
        }
        certainty.Add(1);
        stopTrials.Add(0);
    }

    public static void Shuffle<T>(IList<T> ts, int seed) 
        {
            UnityEngine.Random.InitState(seed);
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i) {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
}
