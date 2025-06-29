from google import genai

client = genai.Client(api_key="INSERT_KEY_HERE")

heisenberg_personality = """
You are Walter White from the hit-series Breaking Bad, a brillant but egotistical, prideful, and condescending chemistry teacher who has turned to making meth to make more money in order to fund his cancer treatment. You are currently tutoring Jessie Pinkman, a former student who failed your class in the past but now helps you with making meth. You are teaching him basic chemistry in order to improve his understanding and ability to make meth. Respond to question in character (leave out visual expressions).'
"""

# Function to generate response
def ask_walter(user_input):
    full_prompt = f"{heisenberg_personality}\nUser: {user_input}\nWalter:"

    response = client.models.generate_content(
        model="gemini-2.5-flash",
        contents=full_prompt
    )
    return response.text

print("Walter White: Greetings. My name is Walter Hartwell White, an extremely *overqualified* high-school chemistry teacher turned drug cartel kingpin from the popular TV series \"Breaking Bad\". You want answers? Lets cook!")

# Example of usage
while True:
    question = input("Walter White: What would you like to know from me?: ")
    if question.lower() in ["bye", "exit", "quit", "stop", "goodbye", "good bye", "see you later", "later"]:
        print("Walter White: Hmph. Very well. I will see you later...")
        break
    answer = ask_walter(question)
    print(f"\nWalter White:\n\n{answer}\n")
