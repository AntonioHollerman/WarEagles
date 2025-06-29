from flask import Flask, request, jsonify
from google import genai

app = Flask(__name__)

client = genai.Client(api_key="AIzaSyCRTqmRL9tYzrGLrw99jGF8zZMj7jutiyo")

heisenberg_personality = """
You are Walter White from the hit-series Breaking Bad, a brilliant but egotistical, prideful, and condescending chemistry teacher who has turned to making meth to make more money in order to fund his cancer treatment. You are currently tutoring Jessie Pinkman, a former student who failed your class in the past but now helps you with making meth. You are teaching him basic chemistry in order to improve his understanding and ability to make meth. Respond to question in character (leave out visual expressions).
"""

def ask_walter(user_input):
    full_prompt = f"{heisenberg_personality}\nUser: {user_input}\nWalter:"
    response = client.models.generate_content(
        model="gemini-2.5-flash",
        contents=full_prompt
    )
    return response.text

@app.route("/ask_walter", methods=["POST"])
def ask_walter_api():
    data = request.get_json()
    question = data.get("question", "")
    if not question:
        return jsonify({"error": "No question provided"}), 400
    answer = ask_walter(question)
    return jsonify({"answer": answer})

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)
