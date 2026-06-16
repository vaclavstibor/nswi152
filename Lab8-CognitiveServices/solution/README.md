# Solution of Lab 8 – Cognitive Services

## Screenshots

### 8a – Computer Vision (analýza obrázku):
![Computer Vision resource](img/01_image_objects_analysis_resource.png)

![Image analysis – konzole](img/01_image_objects_analysis_processing.png)

*(vstupní obrázky v [`assets/`](assets/) – např. `horse.jpg`)*

### 8b – OCR:
![OCR – konzole](img/01_ocr_processing.png)

### 8c – Face Detection:
![Face resource](img/01_face_resource.png)

![Face detection – konzole](img/01_face_processing.png)

### 8d – Speech:
![Speech resource](img/02_speech_resource.png)

![Speech Studio / Foundry nenačte „Vyzkoušet“](img/02_speech_web_not_responding_24hours.png)

[speech.microsoft.com](https://speech.microsoft.com/) a nové AI Foundry u resource typu **CognitiveServices** (`cloudappdev26-speech`) u mě zůstalo na *Načítání…* — typ resource Foundry zatím nepodporuje. Proto **8d** přes **Speech SDK** konzoli [`02_Speech/SpeechDemo`](../02_Speech/SpeechDemo/) místo webového studia.

![Speech SDK – speech-to-text / text-to-speech](img/02_speech_sdk_processing.png)

## Summary

- **Computer Vision** (`cloudappdev26-cv`, Germany West Central): analýza obrázku a OCR přes konzolové projekty; endpoint z portálu (`*.cognitiveservices.azure.com`).
- **Face** (`cloudappdev26-face`): detekce obličejů, vlastní resource.
- **Speech** (`cloudappdev26-speech`): resource v Azure OK, **webové Speech Studio nešlo** → ověřeno přes **Speech SDK** (`dotnet run` v `SpeechDemo`).
