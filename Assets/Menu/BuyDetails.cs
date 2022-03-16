using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class BuyDetails : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text stats;
    
    // Start is called before the first frame update
    void Start()
    {
       ResetText();
    }

    public void RefreshText(IEnemyConfiguration enemyConfiguration)
    {
        title.text = enemyConfiguration.Name;
        description.text = enemyConfiguration.Description;
        stats.text = Textify(enemyConfiguration);

    }
    
    public void ResetText()
    {
        title.text = "";
        description.text = "";
        stats.text = "Stats";
    }
    
    private string Textify(IEnemyConfiguration enemyConfiguration)
    {
        return $"Body\n{Textify(enemyConfiguration.AsBodyEquipment)}\nGun\n{Textify(enemyConfiguration.AsGunEquipment)}";
    }
    
    private string Textify([CanBeNull] IBodyEquipment bodyEquipment)
    {
        var statsExplanation = bodyEquipment?.ModifiersTyped.Select(x => x.Textify());

        if (bodyEquipment == null)
            return "[Nothing]";

        return string.Join(", ", statsExplanation);
    }
    
    private string Textify([CanBeNull] IGunEquipment gunEquipment)
    {
        var statsExplanation = gunEquipment?.ModifiersTyped.Select(x => x.Textify());

        if (gunEquipment == null)
            return "[Nothing]";

        return string.Join(", ", statsExplanation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
