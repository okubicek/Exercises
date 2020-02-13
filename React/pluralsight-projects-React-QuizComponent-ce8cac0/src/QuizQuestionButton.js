import React, {Component} from 'react'

class QuizQuestionButton extends Component{
    constructor(props){
        super(props);
    }

    handleClick(){
        this.props.clickHandler(this, this.props.index);
    }

    render(){
        return(
            <button className={this.props.style} onClick={this.handleClick.bind(this)}>{this.props.button_text}</button>
        );
    }
}

export default QuizQuestionButton
