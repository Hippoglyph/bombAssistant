# bombAssistant

            command := <Set Speak Rate> | <button> | exit | <wires> | <keypads> | <Set Strikes> | <Simon Says> | <Whos on First> | <memory> | <reset>
                        <password> <complicatedWires>

************

            morse code := morsecode | morsecode <letterSequence>
            letterSequence := letter next
            letter := <dot> <letter> | <dot>
            dot := short | long

            complicatedWires := complicatedwires <wires>
            wires := <wire> <wires> | <wire>
            wire := <led> <colors> <star> next | <led> <colors> <star> done
            led := led | ""
            star := star | ""
            colors := <color> <color> | <color>
            color := red | blue | white | yellow | black | green

	    wireseqences := wiresequences

            password := password <letters>
            letters := <letters> letter | letter
            letter := alfa | bravo | charlie | delta | echo | foxtrot | golf | hotel | india | juliett | kilo | lima | mike | november | oscar |
                        papa | quebec | romeo | sierra | tango | uniform | victor | whiskey | xray | yankee | zulu

            mazes := mazes

            memory := memory <number>
            number := 1 | 2 | 3 | 4

            whosonfirst := whosonfirst <letters>

            simonsays := simonsays <colors>
            colors := <colors> <color> | <color>
            color := red | blue | white | yellow | black | green

            setstrikes := setstrikes <number>
            number := 0 | 1 | 2

            keypads := keypads <symbol> <symbol> <symbol> <symbol>
            symbol := a | norwegain | b | blackstar | c | cat | cross | euro | h | halfthree | lambda | lightning | moon | n |
		      nose | omega | pharagraph | psi | q | questionmark | six | smiley | snake | stitches | three | trademark | whitestar

            button := button <color> <text>
            color := red | blue | white | yellow
            text := abort | detonate | hold | press

            Set Speak Rate := setSpeakRateString <number>
            number := -10 | -9 | ... | 8 | 9 | 10

            wires := wires <colors>
            colors := <colors> <color> | <color>
            color := red | blue | white | yellow | black


************

            morseLetter := <dot> <morseLetter | <dot>
            dot := long | short | exit

            wiresequence := done | exit | <color> <letter>
            color := red | blue | black
            letter := alfa | bravo | charlie

            letters := <letters> letter | letter
            letter := alfa | bravo | charlie | delta | echo | foxtrot | golf | hotel | india | juliett | kilo | lima | mike | november | oscar |
                        papa | quebec | romeo | sierra | tango | uniform | victor | whiskey | xray | yankee | zulu | exit | questionmark | empty

            number := 0..10

            coord := <number> <number>
            number := 0...7

            bool := yes | no | true | false