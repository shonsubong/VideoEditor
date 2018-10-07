from flask import Flask
import audio_recognizer

app = Flask(__name__)

@app.route('/')
def hello():
    return 'hello world'

@app.route('/audio_recognize/<string:path>/<string:encoding>/<string:sample_rate>/')
def audio_recognize(path, encoding, sample_rate):
    recog = audio_recognizer.AudioRecognizer()
    return recog.audio_recognize()

if __name__ == '__main__':
    app.run()

