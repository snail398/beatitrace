using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Race 
{
    [SerializeField] private long id;
    [SerializeField] private long trackID;
    [SerializeField] private long bestminute;
    [SerializeField] private long bestsecond;
    [SerializeField] private long bestmsecond;

    public long Id { get => id; set => id = value; }
    public long TrackID { get => trackID; set => trackID = value; }
    public long Bestminute { get => bestminute; set => bestminute = value; }
    public long Bestsecond { get => bestsecond; set => bestsecond = value; }
    public long Bestmsecond { get => bestmsecond; set => bestmsecond = value; }
}
