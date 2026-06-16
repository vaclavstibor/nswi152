# LAB 8d - Speech

*Zadání: Vyzkoušejte Speech Service, otestujte, co vše umí a jak vám rozumí.*

## Speech Studio nefunguje u našeho typu resource

[speech.microsoft.com](https://speech.microsoft.com/) / nové **AI Foundry** u resource typu **CognitiveServices** (klasický Speech z Portálu) u nás **nenačte** modul „Vyzkoušet“ – zůstane na *Načítání…* (viz `solution/img/02_speech_web_not_responding_24hours.png`). Banner říká, že Foundry tento typ resource ještě nepodporuje.

**Řešení pro lab:** konzole **`SpeechDemo`** s [Speech SDK](https://github.com/Azure-Samples/cognitive-services-speech-sdk) (oficiální C# quickstart v repu).

## Spuštění

1. Azure Portal → **cloudappdev26-speech** → **Keys and Endpoint** → **Key 1**, region **Germany West Central**.
2. Vlož klíč do [`SpeechDemo/Program.cs`](SpeechDemo/Program.cs).
3.:

   ```bash
   cd Lab8-CognitiveServices/02_Speech/SpeechDemo
   dotnet run
   ```

4. **1** – řeč z mikrofonu (cs-CZ), **2** – text-to-speech (přehrání věty).

Screenshoty: resource v Portálu + terminál s rozpoznaným textem / TTS → `Lab8-CognitiveServices/solution/img/`.
