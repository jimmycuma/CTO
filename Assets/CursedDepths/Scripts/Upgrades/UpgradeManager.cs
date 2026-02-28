using System.Collections.Generic;
using UnityEngine;
using CursedDepths.Player;
using CursedDepths.Core;
using CursedDepths.Meta;

namespace CursedDepths.Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        private readonly List<UpgradeData> availableUpgrades = new List<UpgradeData>();
        private readonly List<UpgradeData> chosenUpgrades = new List<UpgradeData>();
        private readonly List<SynergyData> synergies = new List<SynergyData>();
        private PlayerController player;
        private MetaProgress metaProgress;

        public IReadOnlyList<UpgradeData> ChosenUpgrades => chosenUpgrades;

        public void Initialize(List<UpgradeData> upgrades, List<SynergyData> synergyData, PlayerController playerController, MetaProgress progress)
        {
            availableUpgrades.Clear();
            availableUpgrades.AddRange(upgrades);
            synergies.Clear();
            synergies.AddRange(synergyData);
            player = playerController;
            metaProgress = progress;
        }

        public UpgradeData[] GetRandomSelection(int count)
        {
            EventBus.RaiseUpgradeSelectionStarted();
            var selection = new UpgradeData[Mathf.Min(count, availableUpgrades.Count)];
            for (int i = 0; i < selection.Length; i++)
            {
                int index = Random.Range(0, availableUpgrades.Count);
                selection[i] = availableUpgrades[index];
                availableUpgrades.RemoveAt(index);
            }
            return selection;
        }

        public void ApplyUpgrade(UpgradeData data)
        {
            if (data == null || player == null)
            {
                return;
            }

            chosenUpgrades.Add(data);
            player.Health.IncreaseMaxHealth(data.bonusHealth);
            player.Shooting.IncreaseDamage(data.bonusDamage);
            player.Shooting.IncreaseAmmo(data.bonusAmmo);
            player.Shooting.IncreaseFireRate(data.bonusFireRate);
            player.Shooting.IncreaseProjectileSpeed(data.bonusProjectileSpeed);
            metaProgress.RegisterUpgrade(data.id);
            ApplySynergies();
            EventBus.RaiseUpgradeSelectionEnded();
        }

        private void ApplySynergies()
        {
            for (int i = 0; i < synergies.Count; i++)
            {
                var synergy = synergies[i];
                bool hasAll = true;
                for (int j = 0; j < synergy.requiredUpgradeIds.Count; j++)
                {
                    if (!metaProgress.HasUpgrade(synergy.requiredUpgradeIds[j]))
                    {
                        hasAll = false;
                        break;
                    }
                }

                if (hasAll)
                {
                    player.Shooting.IncreaseDamage(synergy.bonusDamage);
                    player.Shooting.IncreaseFireRate(synergy.bonusFireRate);
                }
            }
        }
    }
}
