import React, {Component} from 'react'
import QuizQuestionButton from './QuizQuestionButton.js'
import quizStyles from './styles/quiz-styles.js'

class QuizQuestion extends Component{
    constructor(props){
        super(props)
        var buttonStates = this.getInitialStates();
        this.state = {incorrectAnswer: false, buttonStates: buttonStates}
    }

    getInitialStates(){
        return new Array(4).fill('normal');
    }
    
    handleClick(button, index){
        if(button.props.button_text === this.props.quiz_question.answer){
            this.setState({incorrectAnswer: false, buttonStates: this.getInitialStates()})
            this.props.showNextQuestionHandler();
        }else{
            var buttonStates = this.state.buttonStates;            
            buttonStates[index] = 'error';

            this.setState({incorrectAnswer: true, buttonStates: buttonStates});
        }
    }

    render(){
        return(
            <main style={quizStyles.root}>
                <section style={quizStyles.question}>
                    <p>{this.props.quiz_question.instruction_text}</p>
                </section>
                <section className="buttons">
                    {this.props.quiz_question.answer_options.map((answer_option, index) => {
                        return <QuizQuestionButton key={index} index={index} button_text={answer_option} style={this.state.buttonStates[index]}
                        clickHandler={this.handleClick.bind(this)}/>
                    })}
                </section>
                {this.state.incorrectAnswer ? <p className='error'>Sorry, that's not rigth</p> : null}
            </main>
        );
    }
}

export default QuizQuestion