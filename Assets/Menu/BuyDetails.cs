using System.Linq;
using InternalLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu
{
    public class BuyDetails : MonoBehaviour
    {
        [FormerlySerializedAs("title")] public TMP_Text Title;
        [FormerlySerializedAs("description")] public TMP_Text Description;
        [FormerlySerializedAs("stats")] public TMP_Text Stats;
    
        // Start is called before the first frame update
        void Start()
        {
            ResetText();
        }

        public void RefreshText(IEnemyConfiguration enemyConfiguration)
        {
            Title.text = enemyConfiguration.Name;
            Description.text = enemyConfiguration.Description;
            Stats.text = Textify(enemyConfiguration);

        }
    
        public void ResetText()
        {
            Title.text = "";
            Description.text = "";
            Stats.text = "Stats";
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
}
