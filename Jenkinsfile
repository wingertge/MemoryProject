pipeline {
  agent any
  stages {
    stage('NPM Install') {
      steps {
        bat(script: 'npm install', returnStdout: true, returnStatus: true)
      }
    }
  }
}