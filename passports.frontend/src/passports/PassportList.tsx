import React, { FC, ReactElement, useRef, useEffect, useState } from 'react';
import { ICreatePassportDto, Client, PassportDto } from '../api/api';
import { FormControl } from 'react-bootstrap';

const apiClient = new Client('https://localhost:7288');

async function createPassport(Passport: ICreatePassportDto) {
    await apiClient.create('1.0', Passport);
    console.log('Passport is created.');
}

const PassportList: FC<{}> = (): ReactElement => {
    let seriesInput = useRef(null);
    let numberInput = useRef(null);
    const [passports, setPassports] = useState<PassportDto[] | undefined>(undefined);

    async function getPassports() {
        const passportList = await apiClient.getAll(0,100,'1.0');
        setPassports(passportList);
    }

    useEffect(() => {
        setTimeout(getPassports, 500);
    }, []);

    const handleKeyPress = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
            const passport: ICreatePassportDto = {
                series: parseInt(event.currentTarget.value, 10),
                number: parseInt(event.currentTarget.value, 10),
            };
            createPassport(passport);
            event.currentTarget.value = '';
            setTimeout(getPassports, 500);
            getPassports();
        }
    };

    return (
        <div>
            Passports
            <div>
                <FormControl ref={seriesInput} onKeyPress={handleKeyPress} />
                <FormControl ref={numberInput} onKeyPress={handleKeyPress} />
            </div>
            <section>
                {passports?.map((passport) => (
                    <div>{passport.series} {passport.number}</div>
                ))}
            </section>
        </div>
    );
};
export default PassportList;