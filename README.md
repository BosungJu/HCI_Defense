# HCI_Defense

## 1. 개요
    본 프로젝트는 HCI과목의 프로젝트입니다.
    해당 프로젝트는 VR 프로젝트이며 유니티를 사용합니다.

## 2. 명명 규칙
- 폴더명 : 명사
- 파일명 : 파일종류_리소스종류_세부사항 ex) Animation_Archor_Shot
- 클래스 : 파스칼 케이스
- 함수 : 파스칼 케이스
- 변수 : 카멜 케이스
- 프로퍼티 : 파스칼 케이스
- 공백 : 탭

## 3. Developers' Guidelines
    이 Repository 는 개발시 GitHub-flow 를 사용합니다. 
    개발하는 사항이 있는 경우 master에 직접 커밋하는 것이 아닌, 
    feature/add-subtitle 형식의 브랜치 이름을 가지고 작업해 주시고, 
    모든 개발버전의 경우 PR은 develop 으로 PR을 걸어 주십시오. 
    master는 최종 또는 Release Candidate 등의 정상 동작을 보장하는 주요버전에만 사용됩니다. 
    PR하기 전에 무조건 말해주세요.

## 4. commit cases
    1. 버그를 고쳐야 할 때 : fix를 앞에 붙이기. ex) fix: click evt null ref except.
    2. 중간 피쳐 개발 시 : feat를 앞에 붙이기. ex) feat: user move. 