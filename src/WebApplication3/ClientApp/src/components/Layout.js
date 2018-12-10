import React, { Component } from 'react';
//import { Col, Grid, Row } from 'react-bootstrap';
import { Layout, BackTop, Icon } from 'antd';
import { NavMenu } from './NavMenu';
import './Layout.css';

const {
    Header, Content, Footer
} = Layout;
export class _Layout extends Component {
    displayName = _Layout.name

    render() {
        return (
            <Layout>
                <NavMenu />
                <Layout style={{ marginLeft: 200 }}>
                    <Header style={{ background: '#fff', padding: 0 }} />
                    <Content style={{ margin: '24px 16px 0', overflow: 'initial' }}>
                        <div style={{ padding: 24, background: '#fff', textAlign: 'center' }}>
                            {this.props.children}
                        </div>
                        <BackTop>
                            <div className="ant-back-top-inner " title="置顶">
                                <Icon type="arrow-up" theme="outlined" />
                            </div>
                        </BackTop>
                    </Content>
                    <Footer style={{ textAlign: 'center' }}>
                        Ant Design ©2018 Created by Ant UED
                    </Footer>
                </Layout>
            </Layout>
            //  mountNode
        );
    }
}
