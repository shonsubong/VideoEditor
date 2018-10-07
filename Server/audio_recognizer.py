from google.cloud import speech
from google.cloud.speech import enums
from google.cloud.speech import types
import io

class AudioRecognizer:

    def __init__(self):
        self.client = speech.SpeechClient()

    def audio_recognize(self):
        with io.open('D:\\workspace\\speech_ex\\speech_ex\\bin\\Debug\\test.raw', 'rb') as audio_file2:
            content2 = audio_file2.read()

        audio = types.RecognitionAudio(content=content2)
        config = types.RecognitionConfig(
            encoding=enums.RecognitionConfig.AudioEncoding.LINEAR16,
            sample_rate_hertz=8000,
            enable_word_time_offsets = True,
            language_code='ko-KR')

        response = self.client.recognize(config, audio)

        for result in response.results:
            return (u'{}'.format(result.alternatives[0].transcript))
