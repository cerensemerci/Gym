using Basics.Models;

namespace Basics.Services
{
    public class AIService
    {
        public async Task<string> GetExerciseRecommendations(int age, double weight, double height, string gender, string goal)
        {
            // Calculate BMI for more 'intelligent' recommendation
            double heightInMeters = height / 100.0;
            double bmi = weight / (heightInMeters * heightInMeters);
            string bmiStatus = bmi < 18.5 ? "Düşük Kilolu" : (bmi < 25 ? "Normal" : (bmi < 30 ? "Fazla Kilolu" : "Obez"));

            string recommendation = $"### AI Analiz Sonucu\n" +
                                     $"**Mevcut Durum:** BMI {bmi:F1} ({bmiStatus})\n\n" +
                                     $"**Egzersiz Planı:** {goal} hedefiniz için ";

            if (goal.ToLower().Contains("kilo") || bmi > 25)
            {
                recommendation += "haftada 4 gün yüksek yoğunluklu kardiyo (HIIT) ve 2 gün tüm vücut direnç antrenmanı önerilir. Günlük 10.000 adım hedefleyin.\n\n";
                recommendation += "**Beslenme Önerisi:** Günlük kalori alımınızı { (Bazal Metabolizma - 500) } kcal seviyesinde tutun. Protein ağırlıklı beslenin.";
            }
            else if (goal.ToLower().Contains("kas") || goal.ToLower().Contains("güç"))
            {
                recommendation += "haftada 4-5 gün bölgesel ağırlık antrenmanı (Push/Pull/Legs) önerilir. Set arası dinlenmeleri 90 saniyede tutun.\n\n";
                recommendation += "**Beslenme Önerisi:** Kilo başına 2.0g protein tüketin. Kompleks karbonhidratları antrenman öncesi tercih edin.";
            }
            else
            {
                recommendation += "haftada 3 gün fonksiyonel antrenman ve 2 gün yoga/esneklik çalışması önerilir.\n\n";
                recommendation += "**Beslenme Önerisi:** Dengeli Akdeniz diyeti tipinde beslenme, vitamin ve mineral alımına dikkat edin.";
            }

            return await Task.FromResult(recommendation);
        }

        public string GetVisualizationPrompt(string goal, string gender)
        {
            // Simulate AI Image Generation Suggestion
            return $"AI, kullanıcının {goal} hedefine ulaştığı halini {gender} vücut tipinde, spor salonu ışıklandırması altında atletik bir formda simüle ediyor...";
        }
    }
}
